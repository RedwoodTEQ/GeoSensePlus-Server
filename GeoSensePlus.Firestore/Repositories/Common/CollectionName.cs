using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Repositories.Common
{
    public static class CollectionName
    {
        public const string tenants = nameof(CollectionName.tenants);
        public const string geofences = nameof(CollectionName.geofences);
        public const string edges = nameof(CollectionName.edges);
        public const string assets = nameof(CollectionName.assets);
        public const string floorplans = nameof(CollectionName.floorplans);
        public const string edgeMarks = "edgeDevices";
    }
}
