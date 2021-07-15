using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class AreaEntity
    {
        public int AreaId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Area : AreaEntity
    {
        public Floorplan Floorplan { get; set; }
        public Edge Edge { get; set; }

        //public List<Target> CacheTargets { get; set; } = new List<Target>();
    }
}
