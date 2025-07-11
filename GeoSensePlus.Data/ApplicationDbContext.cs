using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Map;
using GeoSensePlus.Data.DatabaseModels.Sensing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GeoSensePlus.Data.DatabaseModels.AlarmEvent;
using GeoSensePlus.Data.DatabaseModels.PubSub;

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

        #region schema: pub_sub
        public DbSet<Topic> Topics { get; set; }
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
    }
}
