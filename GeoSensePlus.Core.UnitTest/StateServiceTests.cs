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

public class StateServiceTests
{
    ITestOutputHelper _output;

    public StateServiceTests(ITestOutputHelper output)
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
        // Arrange
        using var context = CreateDbContext();
        var group = new PointGroup { Name = "Root" };
        context.Add(group);
        await context.SaveChangesAsync();

        var service = new StateService(context);
        
        // Act
        var points = await service.AddPointsAsync(["P1", "P2"], group.Id);

        // Assert
        Assert.Equal(2, points.Count);
        Assert.All(points, p => Assert.Equal(group.Id, p.ParentId));
    }

    [Fact]
    public async Task RemovePointsAsync_RemovesCorrectPoints()
    {
        // Arrange
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

        var service = new StateService(context);
        
        // Act
        var result = await service.RemovePointsAsync(points.Select(p => p.Id));

        // Assert
        Assert.True(result);
        Assert.Empty(group.Points);
    }

    [Fact]
    public async Task MovePointsAsync_MovesPointsToNewGroup()
    {
        // Arrange
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

        var service = new StateService(context);
        
        // Act
        var result = await service.MovePointsAsync(points.Select(p => p.Id), g2.Id);

        // Assert
        Assert.True(result);
        Assert.All(points, p => Assert.Equal(g2.Id, p.ParentId));
    }

    [Fact]
    public async Task GetFullPathOfPointAsync_ReturnsCorrectPath()
    {
        // Arrange
        using var context = CreateDbContext();
        var root = new PointGroup { Name = "Root" };
        var child = new PointGroup { Name = "Child", Parent = root };
        var point = new Point { Name = "Leaf", Parent = child };
        context.AddRange(root, child, point);
        await context.SaveChangesAsync();

        var service = new StateService(context);
        
        // Act
        var path = await service.GetFullPathOfPointAsync(point.Id);

        // Assert
        Assert.Equal("Root/Child/Leaf", path);
    }

    [Fact]
    public async Task AttachValueToPointAsync_SuccessfullyAttachesValue()
    {
        // Arrange
        using var context = CreateDbContext();
        var group = new PointGroup { Name = "Group" };
        var point = new Point { Name = "TestPoint", Parent = group };
        var value = new Value { Name = "TestValue", Type = "string", ValueString = "test" };
        
        context.AddRange(group, point, value);
        await context.SaveChangesAsync();

        var service = new StateService(context);
        
        // Act
        var result = await service.AttachValueToPointAsync(point.Id, value.Id);

        // Assert
        Assert.True(result);
        
        // Assert (verify the attachment)
        var updatedPoint = await context.Set<Point>().FindAsync(point.Id);
        Assert.Equal(value.Id, updatedPoint.ValueId);
    }

    [Fact]
    public async Task AttachValueToPointAsync_FailsWhenPointNotFound()
    {
        // Arrange
        using var context = CreateDbContext();
        var value = new Value { Name = "TestValue", Type = "string", ValueString = "test" };
        context.Add(value);
        await context.SaveChangesAsync();

        var service = new StateService(context);
        
        // Act
        var result = await service.AttachValueToPointAsync(999, value.Id); // Non-existent point ID

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AttachValueToPointAsync_FailsWhenValueNotFound()
    {
        // Arrange
        using var context = CreateDbContext();
        var group = new PointGroup { Name = "Group" };
        var point = new Point { Name = "TestPoint", Parent = group };
        context.AddRange(group, point);
        await context.SaveChangesAsync();

        var service = new StateService(context);
        
        // Act
        var result = await service.AttachValueToPointAsync(point.Id, 999); // Non-existent value ID

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetPointsByValueIdAsync_ReturnsCorrectPoints()
    {
        // Arrange
        using var context = CreateDbContext();
        var group = new PointGroup { Name = "Group" };
        var value = new Value { Name = "SharedValue", Type = "int", IntegerValue = 42 };
        var points = new List<Point> {
            new Point { Name = "Point1", Parent = group, Value = value },
            new Point { Name = "Point2", Parent = group, Value = value },
            new Point { Name = "Point3", Parent = group } // No value attached
        };
        
        context.AddRange(group, value);
        context.AddRange(points);
        await context.SaveChangesAsync();

        var service = new StateService(context);
        
        // Act
        var result = await service.GetPointsByValueIdAsync(value.Id);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Equal(value.Id, p.ValueId));
        Assert.Contains(result, p => p.Name == "Point1");
        Assert.Contains(result, p => p.Name == "Point2");
    }
}

