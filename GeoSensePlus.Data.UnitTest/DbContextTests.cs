using GeoSensePlus.Data.DatabaseModels.Sensing;
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
    public async Task AddSensorToDatabase_Successfully()
    {
        // Arrange
        using var context = CreateDbContext();

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
