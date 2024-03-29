﻿using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Tracking
{
    public class TargetEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// A target is a tracking target, which can be an asset or a person.
    /// </summary>
    public class Target : TargetEntity, IIdAvailable<int>
    {
        //public Area CacheArea { get; set; }

        public List<CellTag> CellTags { get; set; } = new List<CellTag>();
        public List<UwbTag> UwbTags { get; set; } = new List<UwbTag>();
        public List<GpsTag> GpsTags { get; set; } = new List<GpsTag>();

        public int TargetId { get; set; }

        public int GetId()
        {
            return TargetId;
        }
    }
}
