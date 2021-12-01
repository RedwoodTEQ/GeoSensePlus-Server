using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class EdgeDataBase
    {
        [FirestoreDocumentId]
        public string HardwareSerial { get; protected set; } = null!;

        [FirestoreProperty, ServerTimestamp] 
        public Timestamp LastUpdate { get; protected set; }

        [FirestoreProperty]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public ArrayList AssetRefs { get; protected set; } = new ArrayList();

        [FirestoreProperty] 
        public string Name { get; protected set; } = null!;
        
        [FirestoreProperty]
        public string Remarks { get; protected set; } = null!;
    }
    
    [FirestoreData]
    public class EdgeData : EdgeDataBase
    {
        [FirestoreProperty]
        public DocumentReference EdgeMarkerRef { get; protected set; } = null!;
    }
    
    public class EdgeDataJson : EdgeDataBase
    {
        public ReferenceMeta EdgeMarkerRef { get; }
        
        public EdgeDataJson(EdgeData edge)
        {
            HardwareSerial = edge.HardwareSerial;
            Name = edge.Name;
            Remarks = edge.Remarks;
            LastUpdate = edge.LastUpdate;
            foreach (DocumentReference? assetRef in edge.AssetRefs)
            {
                AssetRefs.Add(new ReferenceMeta(assetRef!.Path, assetRef.GetType().ToString()));
            }

            EdgeMarkerRef = new ReferenceMeta(edge.EdgeMarkerRef.Path, edge.EdgeMarkerRef.GetType().ToString());
        }
    }
}
