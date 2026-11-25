using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Sensing;
using GeoSensePlus.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace GeoSensePlus.Data.UnitTest;

public class DbContextTests
{
    ITestOutputHelper _output;
    public DbContextTests(ITestOutputHelper output)
    {
        _output = output;
    }
    private ApplicationDbContext CreateTestContext()
    {
        return ApplicationDbContext.CreateTestContext();
    }

    [Fact]
    public async Task AddSensorToDatabase_Successfully()
    {
        // Arrange
        using var context = CreateTestContext();

        var sensor = new Sensor
        {
            Name = "Test Sensor",
            Type = "Temperature",
        };

        // Act
        context.Sensors.Add(sensor);
        await context.SaveChangesAsync();

        // Assert
        var retrieved = await context.Sensors.FirstOrDefaultAsync(s => s.Name == "Test Sensor");
        Assert.NotNull(retrieved);
        Assert.Equal("Temperature", retrieved.Type);
    }

    [Fact]
    public async Task SaveChanges_PopulatesTimestamps_OnCreateAndUpdate()
    {
        // Arrange: unique name so reruns don't match rows from previous runs
        string name = $"Timestamp Test Site {Guid.NewGuid():N}";
        using var context = CreateTestContext();

        var site = new Site { Name = name, Description = "created" };

        // Act: create the entity
        context.Sites.Add(site);
        await context.SaveChangesAsync();

        // Assert: CreatedAt and UpdatedAt populated on insert (read back from database, not the tracked instance)
        var inserted = await context.Sites.AsNoTracking().FirstAsync(s => s.Name == name);
        DateTimeOffset recentWindowStart = DateTimeOffset.UtcNow.AddMinutes(-5);
        Assert.True(inserted.CreatedAt > recentWindowStart, $"CreatedAt not populated: {inserted.CreatedAt}");
        Assert.True(inserted.UpdatedAt > recentWindowStart, $"UpdatedAt not populated: {inserted.UpdatedAt}");

        // Act: update the entity (site is still tracked by the context)
        await Task.Delay(20);   // ensure UpdatedAt advances by a measurable amount
        site.Description = "updated";
        await context.SaveChangesAsync();

        // Assert: UpdatedAt refreshed, CreatedAt unchanged
        var updated = await context.Sites.AsNoTracking().FirstAsync(s => s.Name == name);
        Assert.Equal("updated", updated.Description);
        Assert.Equal(inserted.CreatedAt.UtcDateTime, updated.CreatedAt.UtcDateTime);
        Assert.True(updated.UpdatedAt > inserted.UpdatedAt, $"UpdatedAt not refreshed: {updated.UpdatedAt} <= {inserted.UpdatedAt}");
    }
}
