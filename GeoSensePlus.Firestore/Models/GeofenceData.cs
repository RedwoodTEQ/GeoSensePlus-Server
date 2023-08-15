using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class GeofenceDataBase
    {
        [FirestoreDocumentId]
        public string Id { get; protected set; } = null!;

        [FirestoreProperty("bounds")]
        public string Bounds { get; protected set; } = null!;

        [FirestoreProperty("centroid")]
        public string Centroid { get; protected set; } = null!;

        [FirestoreProperty("name")]
        public string Name { get; protected set; } = null!;

        [FirestoreProperty("radius")]
        public double Radius { get; protected set; }

        [FirestoreProperty("shapeType")]
        public string ShapeType { get; protected set; } = null!;

        [FirestoreProperty("vertices")]
        public string Vertices { get; protected set; } = null!;
    }
    
    [FirestoreData]
    public class GeofenceData : GeofenceDataBase { }
    public class GeofenceDataJson : GeofenceDataBase { }
}
