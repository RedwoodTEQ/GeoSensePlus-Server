using GeoSensePlus.Data.DatabaseModels.AlarmEvent;
using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Map;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DatabaseModels.Sensing;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection.Emit;

namespace GeoSensePlus.Data.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        #region schema: alarm_event
        public DbSet<Alarm> Alarms { get; set; }
        public DbSet<EventRecord> EventRecords { get; set; }
        #endregion

        #region schema: location
        public DbSet<Area> Areas { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<FloorPlan> FloorPlans { get; set; }
        public DbSet<Site> Sites { get; set; }
        #endregion

        #region schema: map
        public DbSet<Geofence> Geofences { get; set; }
        public DbSet<Marker> Markers { get; set; }
        #endregion

        #region schema: messaging
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Point> Points { get; set; }
        public DbSet<PointGroup> PointGroups { get; set; }
        #endregion

        #region schema: sensing
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        #endregion

        #region schema: tracking
        public DbSet<CellAnchor> CellAnchors { get; set; }
        public DbSet<CellHub> CellHubs { get; set; }
        public DbSet<CellTag> CellTags { get; set; }
        public DbSet<GpsTag> GpsTags { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<UwbAnchor> UwbAnchors { get; set; }
        public DbSet<UwbTag> UwbTags { get; set; }
        #endregion

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// For unit tests, use the appsettings.json file from the startup project (GeoSensePlus.WebApi)
        /// </summary>
        static public ApplicationDbContext CreateTestContext()
        {
            string appSettingsPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../../GeoSensePlus.WebApi"));
            //_output.WriteLine($"Using appsettings path: {appSettingsPath}");

            var config = new ConfigurationBuilder()
                .SetBasePath(appSettingsPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = config.GetConnectionString("PostgresConnection");

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString)
                .Options;

            return new ApplicationDbContext(options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /**
             * Apply all IEntityTypeConfiguration<> classes in this assembly.
             * Note: all IEntityTypeConfiguration<> classes' constructors must be parameterless.
             */
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
