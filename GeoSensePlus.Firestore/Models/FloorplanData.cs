using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    [FirestoreData]
    public class FloorplanData
    {
        [FirestoreDocumentId]
        public string Id { get; set; }

        [FirestoreProperty]
        public string building { get; set; }

        [FirestoreProperty]
        public string filePath {get;set;}

        [FirestoreProperty]
        public string floor { get; set; }

        [FirestoreProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>")]
        public string imageUrl { get; set; }

        [FirestoreProperty]
        public string name { get; set; }
    }
}
