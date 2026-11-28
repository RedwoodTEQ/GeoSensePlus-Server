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

public class PointGroupServiceTests
{
    ITestOutputHelper _output;

    private readonly Mock<ILogger<PointGroupService>> _mockLogger;

    public PointGroupServiceTests(ITestOutputHelper output)
    {
        _output = output;
        _mockLogger = new Mock<ILogger<PointGroupService>>();
    }

    private ApplicationDbContext CreateTestContext()
    {
        return ApplicationDbContext.CreateTestContext();
    }

    private PointGroupService CreateService(ApplicationDbContext context)
    {
        return new PointGroupService(context, _mockLogger.Object);
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
