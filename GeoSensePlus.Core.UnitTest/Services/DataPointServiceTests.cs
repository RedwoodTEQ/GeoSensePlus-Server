namespace GeoSensePlus.Core.UnitTest.Services;

using GeoSensePlus.Core.Services;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class DataPointServiceTests
{
    ITestOutputHelper _output;

    private readonly Mock<ILogger<DataPointService>> _mockLogger;

    public DataPointServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _mockLogger = new Mock<ILogger<DataPointService>>();
    }

    private ApplicationDbContext CreateTestContext()
    {
        return ApplicationDbContext.CreateTestContext();
    }

    private DataPointService CreateService(ApplicationDbContext context)
    {
        return new DataPointService(context, _mockLogger.Object, new PointGroupService(context, Mock.Of<ILogger<PointGroupService>>()));
    }

    [Fact]
    public async Task AddPointsAsync_AddsPointsToGroup()
    {
        // Arrange
        int groupId;
        using (var arrangeContext = CreateTestContext())
        {
            var group = new PointGroup { Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}" };
            arrangeContext.Add(group);
            await arrangeContext.SaveChangesAsync();
            groupId = group.Id;
        }

        // Act
        List<DataPoint> points;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            points = await service.AddPointsAsync(["P1", "P2"], groupId);
        }

        // Assert
        using (var assertContext = CreateTestContext())
        {
            var group = await assertContext.PointGroups.Include(g => g.DataPoints).FirstOrDefaultAsync(g => g.Id == groupId);
            Assert.NotNull(group);
            Assert.Equal(2, group.DataPoints.Count);
            Assert.All(group.DataPoints, p => Assert.Equal(groupId, p.ParentId));
        }

        // Verify logging
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Skipping duplicate point name")),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
            ),Times.Never);
    }

    [Fact]
    public async Task RemovePointsAsync_RemovesCorrectPoints()
    {
        // Arrange
        int groupId;
        int[] pointIds;
        using (var arrangeContext = CreateTestContext())
        {
            var group = new PointGroup { Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}" };
            var points = new[] {
                new DataPoint { Name = "P1", Parent = group },
                new DataPoint { Name = "P2", Parent = group }
            };
            arrangeContext.Add(group);
            arrangeContext.AddRange(points);
            await arrangeContext.SaveChangesAsync();
            groupId = group.Id;
            pointIds = points.Select(p => p.Id).ToArray();
            Assert.Equal(2, group.DataPoints.Count);
        }

        // Act
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            await service.RemovePointsAsync(pointIds);
        }

        // Assert
        using (var assertContext = CreateTestContext())
        {
            var group = await assertContext.PointGroups.Include(g => g.DataPoints).FirstOrDefaultAsync(g => g.Id == groupId);
            Assert.NotNull(group);
            Assert.Empty(group.DataPoints);
        }
    }

    [Fact]
    public async Task MovePointsAsync_MovesPointsToNewGroup()
    {
        // Arrange
        int g1Id, g2Id;
        int[] pointIds;
        using (var arrangeContext = CreateTestContext())
        {
            var g1 = new PointGroup { Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}" };
            var g2 = new PointGroup { Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}" };
            var points = new[] {
                new DataPoint { Name = "P1", Parent = g1 },
                new DataPoint { Name = "P2", Parent = g1 }
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
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            await service.MovePointsAsync(pointIds, g2Id);
        }

        // Assert
        using (var assertContext = CreateTestContext())
        {
            var points = await assertContext.DataPoints.Where(p => pointIds.Contains(p.Id)).ToListAsync();
            Assert.All(points, p => Assert.Equal(g2Id, p.ParentId));
        }
    }

    [Fact]
    public async Task GetFullPathOfPointAsync_ReturnsCorrectPath()
    {
        // Arrange
        int pointId;
        string rootName = $"Root_{Guid.NewGuid().ToString("N")[..6]}";
        using (var arrangeContext = CreateTestContext())
        {
            var root = new PointGroup { Name =  rootName};
            var child = new PointGroup { Name = "Child", Parent = root };
            var point = new DataPoint { Name = "Leaf", Parent = child };
            arrangeContext.AddRange(root, child, point);
            await arrangeContext.SaveChangesAsync();
            pointId = point.Id;
        }

        // Act
        string path;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            path = await service.GetFullPathOfPointAsync(pointId);
        }

        // Assert
        Assert.Equal($"{rootName}/Child/Leaf", path);
    }

    [Fact]
    public async Task FindPointByPathAsync_ReturnsCorrectPoint()
    {
        // Arrange
        DataPoint testPoint;
        string rootName = $"Root_{Guid.NewGuid().ToString("N")[..6]}";
        using (var arrangeContext = CreateTestContext())
        {
            var root = new PointGroup { Name = rootName };
            var child = new PointGroup { Name = "Child", Parent = root };
            testPoint = new DataPoint { Name = "TestPoint", Parent = child };
            arrangeContext.AddRange(root, child, testPoint);
            await arrangeContext.SaveChangesAsync();
        }

        // Act
        DataPoint foundPoint;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            foundPoint = await service.FindPointByPathAsync($"{rootName}/Child/TestPoint");
        }

        // Assert
        Assert.NotNull(foundPoint);
        Assert.Equal(testPoint.Id, foundPoint.Id);
        Assert.Equal(testPoint.Name, foundPoint.Name);
        Assert.Equal("Child", foundPoint.Parent.Name);
        Assert.Equal(rootName, foundPoint.Parent.Parent.Name);
    }

    [Fact]
    public async Task FindPointByPathAsync_ReturnsNullForInvalidPath()
    {
        // Arrange
        using (var arrangeContext = CreateTestContext())
        {
            var root = new PointGroup { Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}" };
            var child = new PointGroup { Name = "Child", Parent = root };
            var point = new DataPoint { Name = "TestPoint", Parent = child };
            arrangeContext.AddRange(root, child, point);
            await arrangeContext.SaveChangesAsync();
        }

        // Act
        DataPoint foundPoint;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            foundPoint = await service.FindPointByPathAsync("Root/Child/NonExistentPoint");
        }

        // Assert
        Assert.Null(foundPoint);
    }

    [Fact]
    public async Task FindPointByPathAsync_ReturnsNullForInvalidGroupPath()
    {
        // Arrange
        using (var arrangeContext = CreateTestContext())
        {
            var root = new PointGroup { Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}" };
            var child = new PointGroup { Name = "Child", Parent = root };
            var point = new DataPoint { Name = "TestPoint", Parent = child };
            arrangeContext.AddRange(root, child, point);
            await arrangeContext.SaveChangesAsync();
        }

        // Act
        DataPoint foundPoint;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            foundPoint = await service.FindPointByPathAsync("Root/InvalidGroup/TestPoint");
        }

        // Assert
        Assert.Null(foundPoint);
    }
}
