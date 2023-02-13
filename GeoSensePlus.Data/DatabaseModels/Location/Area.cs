﻿using GeoSensePlus.Data.DatabaseModels.Base;
using GeoSensePlus.Data.DatabaseModels.Tracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location
{

    public class AreaEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CellAnchorId { get; set; }        // rename from EdgeId
    }

    public class Area : AreaEntity, IIdAvailable<int>
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
            ctx.Areas.Add(new Area { Name = name });
            ctx.SaveChanges();
        }
        public static void Create(ApplicationDbContext ctx, string name, string description)
        {
            ctx.Areas.Add(new Area { Name = name, Description = description });
            ctx.SaveChanges();
        }
    }
}
