using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class MeasureEntity
    {
        public int MetricId { get; set; }

        /// <summary>
        /// temperature, humidity etc.
        /// </summary>
        public string Type { get; set; }

        public double Value { get; set; }

        public string Unit { get; set; }
    }

    public class Measure : MeasureEntity
    {
        //public Area CachedArea { get; set; }
        public List<Sensor> Sensors { get; set; } = new List<Sensor>();
    }
}
