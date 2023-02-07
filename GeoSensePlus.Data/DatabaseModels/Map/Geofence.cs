using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels.Map
{
    public class GeofenceEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Empty: indicate it's an outdoor POI or a map marker
        /// Rectangle
        /// Circle
        /// </summary>
        public string Shape { get; set; }
    }

    public class Geofence : GeofenceEntity, IIdAvailable<int>
    {
        public int GeofenceId { get; set; }

        public int GetId()
        {
            return GeofenceId;
        }
    }
}
