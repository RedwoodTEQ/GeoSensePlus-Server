using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class TargetEntity : IIdAvailable<int>
    {
        public int TargetId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GetId()
        {
            return TargetId;
        }
    }

    public class Target : TargetEntity
    {
        //public Area CacheArea { get; set; }

        public List<CellTag> CellTags { get; set; } = new List<CellTag>();
        public List<CoordinateTag> CoordinateTags { get; set; } = new List<CoordinateTag>();
        public List<GpsTag> GpsTags { get; set; } = new List<GpsTag>();
    }
}
