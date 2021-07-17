using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{

    public class AreaEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? EdgeId { get; set; }

    }

    public class Area : AreaEntity, IIdAvailable<int>
    {
        public Floorplan Floorplan { get; set; }
        public Edge Edge { get; set; }

        public int AreaId { get; set; }

        public int GetId()
        {
            return AreaId;
        }
        //public List<Target> CacheTargets { get; set; } = new List<Target>();
    }
}
