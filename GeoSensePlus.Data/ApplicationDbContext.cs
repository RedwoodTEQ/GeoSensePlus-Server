using GeoSensePlus.Data.DatabaseModels.AlarmEvent;
using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Map;
using GeoSensePlus.Data.DatabaseModels.Messaging;
using GeoSensePlus.Data.DatabaseModels.Sensing;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GeoSensePlus.Data
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
        public DbSet<Topic> Topics { get; set; }
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call the base method to ensure Identity tables are created

            modelBuilder.Entity<PointGroup>()
                .HasOne(g => g.Parent)
                .WithMany(g => g.Children)
                .HasForeignKey(g => g.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Point>()
                .HasOne(i => i.Parent)
                .WithMany(g => g.Points)
                .HasForeignKey(i => i.ParentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
