using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    [FirestoreData]
    public class AssetData
    {
        [FirestoreDocumentId]
        public string MacAddress { get; set; }      // used as asset ID

        [FirestoreProperty, ServerTimestamp]
        public Timestamp LastReportTime { get; set; }

        [FirestoreProperty]
        public int SignalStrength { get; set; }

        [FirestoreProperty]
        public int BatteryLevel { get; set; }

        [FirestoreProperty]
        public DocumentReference EdgeRef { get; set; }
    }
}
