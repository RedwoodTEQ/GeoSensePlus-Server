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

        /// <summary>
        /// temperature, humidity etc.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The latest reported value
        /// </summary>
        public double Value { get; set; }

        public string Unit { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class Sensor : SensorEntity
    {
        //public Area CachedArea { get; set; }
        public Measure Measure { get; set; }
        public Edge Edge { get; set; }
    }
}
