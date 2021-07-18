using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    /// <summary>
    /// Represent a "field" in influxdb
    /// </summary>
    public class SensorEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// temperature, humidity etc.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Multiple lables are separated by ';'
        /// Used as influxdb tags
        /// </summary>
        public string Labels { get; set; }

        /// <summary>
        /// The latest reported value
        /// </summary>
        public double Value { get; set; }

        public string Unit { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class Sensor : SensorEntity, IIdAvailable<int>
    {
        //public Area CachedArea { get; set; }
        public Measure Measure { get; set; }
        public Edge Edge { get; set; }

        public int SensorId { get; set; }

        public int GetId()
        {
            return SensorId;
        }
    }
}
