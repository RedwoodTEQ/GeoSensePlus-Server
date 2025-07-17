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
        int groupId;
        using (var arrangeContext = CreateDbContext())
        {
            var group = new PointGroup { Name = "Root" };
            arrangeContext.Add(group);
            await arrangeContext.SaveChangesAsync();
            groupId = group.Id;
        }

        // Act
        List<Point> points;
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            points = await service.AddPointsAsync(["P1", "P2"], groupId);
        }

        // Assert
        using (var assertContext = CreateDbContext())
        {
            var group = await assertContext.PointGroups.Include(g => g.Points).FirstOrDefaultAsync(g => g.Id == groupId);
            Assert.NotNull(group);
            Assert.Equal(2, group.Points.Count);
            Assert.All(group.Points, p => Assert.Equal(groupId, p.ParentId));
        }
    }

    [Fact]
    public async Task RemovePointsAsync_RemovesCorrectPoints()
    {
        // Arrange
        int groupId;
        int[] pointIds;
        using (var arrangeContext = CreateDbContext())
        {
            var group = new PointGroup { Name = "Root" };
            var points = new[] {
                new Point { Name = "P1", Parent = group },
                new Point { Name = "P2", Parent = group }
            };
            arrangeContext.Add(group);
            arrangeContext.AddRange(points);
            await arrangeContext.SaveChangesAsync();
            groupId = group.Id;
            pointIds = points.Select(p => p.Id).ToArray();
            Assert.Equal(2, group.Points.Count);
        }

        // Act
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            await service.RemovePointsAsync(pointIds);
        }

        // Assert
        using (var assertContext = CreateDbContext())
        {
            var group = await assertContext.PointGroups.Include(g => g.Points).FirstOrDefaultAsync(g => g.Id == groupId);
            Assert.NotNull(group);
            Assert.Empty(group.Points);
        }
    }

    [Fact]
    public async Task MovePointsAsync_MovesPointsToNewGroup()
    {
        // Arrange
        int g1Id, g2Id;
        int[] pointIds;
        using (var arrangeContext = CreateDbContext())
        {
            var g1 = new PointGroup { Name = "G1" };
            var g2 = new PointGroup { Name = "G2" };
            var points = new[] {
                new Point { Name = "P1", Parent = g1 },
                new Point { Name = "P2", Parent = g1 }
            };
            arrangeContext.AddRange(g1, g2);
            arrangeContext.AddRange(points);
            await arrangeContext.SaveChangesAsync();
            g1Id = g1.Id;
            g2Id = g2.Id;
            pointIds = points.Select(p => p.Id).ToArray();
            Assert.All(points, p => Assert.Equal(g1Id, p.ParentId));
        }

        // Act
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            await service.MovePointsAsync(pointIds, g2Id);
        }

        // Assert
        using (var assertContext = CreateDbContext())
        {
            var points = await assertContext.Points.Where(p => pointIds.Contains(p.Id)).ToListAsync();
            Assert.All(points, p => Assert.Equal(g2Id, p.ParentId));
        }
    }

    [Fact]
    public async Task GetFullPathOfPointAsync_ReturnsCorrectPath()
    {
        // Arrange
        int pointId;
        using (var arrangeContext = CreateDbContext())
        {
            var root = new PointGroup { Name = "Root" };
            var child = new PointGroup { Name = "Child", Parent = root };
            var point = new Point { Name = "Leaf", Parent = child };
            arrangeContext.AddRange(root, child, point);
            await arrangeContext.SaveChangesAsync();
            pointId = point.Id;
        }

        // Act
        string path;
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            path = await service.GetFullPathOfPointAsync(pointId);
        }

        // Assert
        Assert.Equal("Root/Child/Leaf", path);
    }

    [Fact]
    public async Task AttachValueToPointAsync_SuccessfullyAttachesValue()
    {
        // Arrange
        int pointId, valueId;
        using (var arrangeContext = CreateDbContext())
        {
            var group = new PointGroup { Name = "Group" };
            var point = new Point { Name = "TestPoint", Parent = group };
            var value = new Value { Name = "TestValue", Type = "string", ValueString = "test" };
            arrangeContext.AddRange(group, point, value);
            await arrangeContext.SaveChangesAsync();
            pointId = point.Id;
            valueId = value.Id;
        }

        // Act
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            await service.AttachValueToPointAsync(pointId, valueId);
        }

        // Assert
        using (var assertContext = CreateDbContext())
        {
            var updatedPoint = await assertContext.Points.FindAsync(pointId);
            Assert.Equal(valueId, updatedPoint.ValueId);
        }
    }

    [Fact]
    public async Task AttachValueToPointAsync_FailsWhenPointNotFound()
    {
        // Arrange
        int valueId;
        using (var arrangeContext = CreateDbContext())
        {
            var value = new Value { Name = "TestValue", Type = "string", ValueString = "test" };
            arrangeContext.Add(value);
            await arrangeContext.SaveChangesAsync();
            valueId = value.Id;
        }

        // Act
        bool result;
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            result = await service.AttachValueToPointAsync(999, valueId); // Non-existent point ID
        }

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task AttachValueToPointAsync_FailsWhenValueNotFound()
    {
        // Arrange
        int pointId;
        using (var arrangeContext = CreateDbContext())
        {
            var group = new PointGroup { Name = "Group" };
            var point = new Point { Name = "TestPoint", Parent = group };
            arrangeContext.AddRange(group, point);
            await arrangeContext.SaveChangesAsync();
            pointId = point.Id;
        }

        // Act
        bool result;
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            result = await service.AttachValueToPointAsync(pointId, 999); // Non-existent value ID
        }

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetPointsByValueIdAsync_ReturnsCorrectPoints()
    {
        // Arrange
        int valueId;
        using (var arrangeContext = CreateDbContext())
        {
            var group = new PointGroup { Name = "Group" };
            var value = new Value { Name = "SharedValue", Type = "int", IntegerValue = 42 };
            var points = new List<Point> {
                new Point { Name = "Point1", Parent = group, Value = value },
                new Point { Name = "Point2", Parent = group, Value = value },
                new Point { Name = "Point3", Parent = group } // No value attached
            };
            arrangeContext.AddRange(group, value);
            arrangeContext.AddRange(points);
            await arrangeContext.SaveChangesAsync();
            valueId = value.Id;
        }

        // Act
        List<Point> result;
        using (var actContext = CreateDbContext())
        {
            var service = new StateService(actContext);
            result = await service.GetPointsByValueIdAsync(valueId);
        }

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.Equal(valueId, p.ValueId));
        Assert.Contains(result, p => p.Name == "Point1");
        Assert.Contains(result, p => p.Name == "Point2");
    }
}

