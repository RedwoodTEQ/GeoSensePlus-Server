using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class GpsTagEntity : IIdAvailable<int>
    {
        public int GpsTagId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public DateTime TimeStamp { get; set; }

        public int GetId()
        {
            return GpsTagId;
        }
    }

    public class GpsTag : GpsTagEntity
    {
        public Target Target { get; set; }
    }
}
