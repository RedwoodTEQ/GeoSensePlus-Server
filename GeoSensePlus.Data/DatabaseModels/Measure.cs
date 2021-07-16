using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class MeasureEntity
    {
        public int MeasureId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Measure : MeasureEntity
    {
        //public Area CachedArea { get; set; }
        public List<Sensor> Sensors { get; set; } = new List<Sensor>();
    }
}
