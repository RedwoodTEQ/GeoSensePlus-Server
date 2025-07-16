namespace GeoSensePlus.Core.UnitTest;

using GeoSensePlus.Core.Services;
using GeoSensePlus.Data;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
// Unit tests for PointService methods

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class PointServiceTests
{
    ITestOutputHelper _output;

    public PointServiceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    private ApplicationDbContext CreateDbContext()
    {
        string appSettingsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../GeoSensePlus.WebApi"));
        _output.WriteLine($"Using appsettings path: {appSettingsPath}");

        var config = new ConfigurationBuilder()
            .SetBasePath(appSettingsPath)
            .AddJsonFile("appsettings.json") // if needed: AddJsonFile(@"..\..\..\ThirdProject\appsettings.json")
            .Build();

        var connectionString = config.GetConnectionString("PostgresConnection");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new ApplicationDbContext(options);
    }


    [Fact]
    public async Task AddPointsAsync_AddsPointsToGroup()
    {
        using var context = CreateDbContext();
        var group = new PointGroup { Name = "Root" };
        context.Add(group);
        await context.SaveChangesAsync();

        var service = new PointService(context);
        var points = await service.AddPointsAsync(["P1", "P2"], group.Id);

        Assert.Equal(2, points.Count);
        Assert.All(points, p => Assert.Equal(group.Id, p.ParentId));
    }

    [Fact]
    public async Task RemovePointsAsync_RemovesCorrectPoints()
    {
        using var context = CreateDbContext();
        var group = new PointGroup { Name = "Root" };
        var points = new[] {
            new Point { Name = "P1", Parent = group },
            new Point { Name = "P2", Parent = group }
        };
        context.Add(group);
        context.AddRange(points);
        await context.SaveChangesAsync();

        Assert.Equal(2, group.Points.Count);

        var service = new PointService(context);
        var result = await service.RemovePointsAsync(points.Select(p => p.Id));

        Assert.True(result);
        Assert.Empty(group.Points);
    }

    [Fact]
    public async Task MovePointsAsync_MovesPointsToNewGroup()
    {
        using var context = CreateDbContext();
        var g1 = new PointGroup { Name = "G1" };
        var g2 = new PointGroup { Name = "G2" };
        var points = new[] {
            new Point { Name = "P1", Parent = g1 },
            new Point { Name = "P2", Parent = g1 }
        };
        context.AddRange(g1, g2);
        context.AddRange(points);
        await context.SaveChangesAsync();

        Assert.All(points, p => Assert.Equal(g1.Id, p.ParentId));

        var service = new PointService(context);
        var result = await service.MovePointsAsync(points.Select(p => p.Id), g2.Id);

        Assert.True(result);
        Assert.All(points, p => Assert.Equal(g2.Id, p.ParentId));
    }

    [Fact]
    public async Task GetFullPathOfPointAsync_ReturnsCorrectPath()
    {
        using var context = CreateDbContext();
        var root = new PointGroup { Name = "Root" };
        var child = new PointGroup { Name = "Child", Parent = root };
        var point = new Point { Name = "Leaf", Parent = child };
        context.AddRange(root, child, point);
        await context.SaveChangesAsync();

        var service = new PointService(context);
        var path = await service.GetFullPathOfPointAsync(point.Id);

        Assert.Equal("Root/Child/Leaf", path);
    }
}

