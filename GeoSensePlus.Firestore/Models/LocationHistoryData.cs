using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class LocationHistoryDataBase
    {
        [FirestoreDocumentId]
        public string Id { get; protected set; } = null!;

        [FirestoreProperty("location")]
        public GeoPoint Location { get; protected set; }

        [FirestoreProperty("timestamp")] 
        public Timestamp Timestamp { get; protected set; }
    }
    
    [FirestoreData]
    public class LocationHistoryData : LocationHistoryDataBase { }
    
    public class LocationHistoryDataJson : LocationHistoryDataBase { }
}