using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class  FloorplanEntity
    {
        public int FloorplanId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileLocation { get; set; }
    }

    public class Floorplan : FloorplanEntity
    {
        public Building Building { get; set; }
        public List<Area> Areas { get; set; } = new List<Area>();
    }
}
