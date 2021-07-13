using GeoSensePlus.Data.DatabaseModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeoSensePlus.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<AlarmEvent> AlarmEvents { get; set; }
        public DbSet<BleEdge> BleEdges { get; set; }
        public DbSet<BleTag> BleTags { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floorplan> Floorplans { get; set; }
        public DbSet<Gateway> Gateway { get; set; }
        public DbSet<Geofence> Geofences { get; set; }
        public DbSet<GpsTag> GpsTags { get; set; }
        public DbSet<Target> Targets { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
    }
}
