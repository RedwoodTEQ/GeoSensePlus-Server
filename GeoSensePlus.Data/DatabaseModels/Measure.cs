using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class MeasureEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Multiple lables are separated by ';'
        /// Used as influxdb tags
        /// </summary>
        public string Labels { get; set; }
    }

    public class Measure : MeasureEntity, IIdAvailable<int>
    {
        //public Area CachedArea { get; set; }
        public List<Sensor> Sensors { get; set; } = new List<Sensor>();

        public int MeasureId { get; set; }

        public int GetId()
        {
            return MeasureId;
        }
    }
}
