using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    [FirestoreData]
    public class EdgeMarkerData
    {
        [FirestoreProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ArrayList location { get; set; }

        [FirestoreProperty]
        public string name { get; set; }

        [FirestoreProperty]
        public DocumentReference EdgeRef { get; set; }
    }
}
