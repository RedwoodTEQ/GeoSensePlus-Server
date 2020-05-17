using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    [FirestoreData]
    public class EdgeData
    {
        [FirestoreDocumentId]
        public string HardwareSerial { get; set; }

        [FirestoreProperty, ServerTimestamp]
        public Timestamp LastUpdate { get; set; }

        [FirestoreProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ArrayList AssetRefs { get; set; } = new ArrayList();

        [FirestoreProperty]
        public DocumentReference EdgeMarkerRef { get; set; }
    }
}
