using GeoSensePlus.Data.DatabaseModels;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using GeoSensePlus.Data.DatabaseModels.Location;
using GeoSensePlus.Data.DatabaseModels.Map;
using GeoSensePlus.Data.DatabaseModels.Sensing;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeoSensePlus.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<AlarmEvent> AlarmEvents { get; set; }
        public DbSet<CellAnchor> CellAnchors { get; set; }
        public DbSet<CellTag> CellTags { get; set; }
        public DbSet<UwbAnchor> UwbAnchors { get; set; }
        public DbSet<UwbTag> UwbTags { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floorplan> Floorplans { get; set; }
        public DbSet<Gateway> Gateway { get; set; }
        public DbSet<Geofence> Geofences { get; set; }
        public DbSet<GpsTag> GpsTags { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Sensor> Sensors { get; set; }
        public DbSet<Measure> Measures { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
    }
}
