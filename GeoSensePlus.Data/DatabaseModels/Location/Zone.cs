using GeoSensePlus.Data.DatabaseModels.Base;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location
{

    public class ZoneEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CellAnchorId { get; set; }        // rename from EdgeId
    }

    /// <summary>
    /// Renamed from "Area".
    /// A zone is an indoor area, which can be a room, a corridor etc.
    /// </summary>
    public class Zone : ZoneEntity, IIdAvailable<int>
    {
        public Floorplan Floorplan { get; set; }
        public CellAnchor CellAnchor { get; set; }

        public int AreaId { get; set; }

        public int GetId()
        {
            return AreaId;
        }
        //public List<Target> CacheTargets { get; set; } = new List<Target>();

        public static void Create(ApplicationDbContext ctx, string name)
        {
            ctx.Zones.Add(new Zone { Name = name });
            ctx.SaveChanges();
        }
        public static void Create(ApplicationDbContext ctx, string name, string description)
        {
            ctx.Zones.Add(new Zone { Name = name, Description = description });
            ctx.SaveChanges();
        }
    }
}
