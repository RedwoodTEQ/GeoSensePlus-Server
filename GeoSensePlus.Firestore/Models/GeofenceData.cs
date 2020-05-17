using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    [FirestoreData]
    public class GeofenceData
    {
        [FirestoreProperty]
        public string Bounds { get; set; }

        [FirestoreProperty]
        public string Centroid { get; set; }

        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public double Radius { get; set; }

        [FirestoreProperty]
        public string ShapeType { get; set; }

        [FirestoreProperty]
        public string Vertices { get; set; }
    }
}
