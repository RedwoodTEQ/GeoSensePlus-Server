namespace GeoSensePlus.Core.UnitTest.Services;

using GeoSensePlus.Core.Services;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System;
// Unit tests for PointService methods

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

public class DirectoryServiceTests
{
    ITestOutputHelper _output;

    private readonly Mock<ILogger<DirectoryService>> _mockLogger;

    public DirectoryServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _mockLogger = new Mock<ILogger<DirectoryService>>();
    }

    private ApplicationDbContext CreateTestContext()
    {
        return ApplicationDbContext.CreateTestContext();
    }

    private DirectoryService CreateService(ApplicationDbContext context)
    {
        return new DirectoryService(context, _mockLogger.Object);
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

    /// <summary>
    /// Regression test for GetAllChildGroupsFlatAsync: the recursive CTE passed to FromSqlRaw must return
    /// ALL columns mapped on PointGroup, otherwise EF Core's composed projection fails with Postgres 42703
    /// (e.g. "column g.created_at does not exist"). Exercises the Include/ThenInclude path as well.
    /// </summary>
    [Fact]
    public async Task GetNestedTreeAsync_ReturnsSubtreeWithPointsAndChannels()
    {
        // Arrange
        int rootId;
        string rootName = $"Root_{Guid.NewGuid().ToString("N")[..6]}";
        using (var arrangeContext = CreateTestContext())
        {
            var root = new PointGroup { Name = rootName };
            var child = new PointGroup { Name = "Child", Parent = root };
            var point = new DataPoint { Name = "Leaf", Parent = child };
            var channel = new DataChannel { DataPoint = point };
            arrangeContext.AddRange(root, child, point, channel);
            await arrangeContext.SaveChangesAsync();
            rootId = root.Id;
        }

        // Act
        List<PointGroup> tree;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            tree = await service.GetNestedTreeAsync(rootId);
        }

        // Assert
        var rootNode = Assert.Single(tree);
        Assert.Equal(rootId, rootNode.Id);
        var childNode = Assert.Single(rootNode.Children);
        Assert.Equal("Child", childNode.Name);
        var leafPoint = Assert.Single(childNode.DataPoints);
        Assert.Equal("Leaf", leafPoint.Name);
        _ = Assert.Single(leafPoint.DataChannels);
    }

    /// <summary>
    /// GetNestedTreeAsync without rootId (null) should return all root groups of the forest,
    /// each with its nested children populated.
    /// </summary>
    [Fact]
    public async Task GetNestedTreeAsync_WithoutRootId_ReturnsAllTrees()
    {
        // Arrange - create two distinct roots, each with a child
        string root1Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}";
        string root2Name = $"Root_{Guid.NewGuid().ToString("N")[..6]}";
        int root1Id, root2Id;
        using (var arrangeContext = CreateTestContext())
        {
            var root1 = new PointGroup { Name = root1Name };
            var root2 = new PointGroup { Name = root2Name };
            var child1 = new PointGroup { Name = "Child1", Parent = root1 };
            var child2 = new PointGroup { Name = "Child2", Parent = root2 };
            arrangeContext.AddRange(root1, root2, child1, child2);
            await arrangeContext.SaveChangesAsync();
            root1Id = root1.Id;
            root2Id = root2.Id;
        }

        // Act
        List<PointGroup> forest;
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            forest = await service.GetNestedTreeAsync();
        }

        // Assert - the shared test DB may contain groups from other tests, so use "contains" assertions
        var node1 = forest.FirstOrDefault(g => g.Id == root1Id);
        var node2 = forest.FirstOrDefault(g => g.Id == root2Id);
        Assert.NotNull(node1);
        Assert.NotNull(node2);
        var childNode1 = Assert.Single(node1.Children);
        Assert.Equal("Child1", childNode1.Name);
        var childNode2 = Assert.Single(node2.Children);
        Assert.Equal("Child2", childNode2.Name);
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

    [Fact]
    public async Task AddGroupAsync_ThrowsForDuplicateRootName()
    {
        // Arrange
        string rootName = $"Root_{Guid.NewGuid().ToString("N")[..6]}";
        using (var arrangeContext = CreateTestContext())
        {
            var root = new PointGroup { Name = rootName };
            arrangeContext.Add(root);
            await arrangeContext.SaveChangesAsync();
        }

        // Act & Assert
        using (var actContext = CreateTestContext())
        {
            var service = CreateService(actContext);
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => 
                service.AddGroupAsync(rootName));
            Assert.Contains($"A group with name '{rootName}' already exists", ex.Message);
        }
    }
}

