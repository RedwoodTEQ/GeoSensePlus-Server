using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class FloorplanEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileLocation { get; set; }
    }

    public class Floorplan : FloorplanEntity, IIdAvailable<int>
    {
        public Building Building { get; set; }
        public List<Area> Areas { get; set; } = new List<Area>();

        public int FloorplanId { get; set; }

        public int GetId()
        {
            return FloorplanId;
        }
    }
}
