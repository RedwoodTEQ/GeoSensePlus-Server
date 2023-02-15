using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Location
{
    public class FloorplanEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileLocation { get; set; }

        /// <summary>
        /// Floor level
        /// </summary>
        public int Level { get; set; }
    }

    public class Floorplan : FloorplanEntity, IIdAvailable<int>
    {
        public Level Level { get; set; }
        public List<Zone> Zones { get; set; } = new List<Zone>();

        public int FloorplanId { get; set; }

        public int GetId()
        {
            return FloorplanId;
        }
    }
}
