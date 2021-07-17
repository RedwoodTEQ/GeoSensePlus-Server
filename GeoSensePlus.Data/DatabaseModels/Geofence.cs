using GeoSensePlus.Data.DatabaseModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Data.DatabaseModels
{
    public class GeofenceEntity : IIdAvailable<int>
    {
        public int GeofenceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int GetId()
        {
            return GeofenceId;
        }
    }

    public class Geofence : GeofenceEntity
    {
    }
}
