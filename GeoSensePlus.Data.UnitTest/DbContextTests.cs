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
}
