using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class SensorEntity
    {
        public int SensorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Sensor : SensorEntity
    {
        //public Area CachedArea { get; set; }
        public Measure Metric { get; set; }
        public Edge Edge { get; set; }
    }
}
