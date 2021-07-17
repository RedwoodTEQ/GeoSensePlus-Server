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
        public int EdgeId { get; set; }
    }

    public class Area : AreaEntity
    {
        public Area() { }

        public Area(AreaEntity areaEntity)
        {
            this.Name = areaEntity.Name;
            this.Description = areaEntity.Description;
        }

        public Floorplan Floorplan { get; set; }
        public Edge Edge { get; set; }

        //public List<Target> CacheTargets { get; set; } = new List<Target>();
    }
}
