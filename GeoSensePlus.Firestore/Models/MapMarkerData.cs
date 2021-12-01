using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class MapMarkerDataBase
    {
        [FirestoreDocumentId]
        public string Id { get; protected set; } = null!;
        
        [FirestoreProperty("location")]
        public GeoPoint Location { get; protected set; }

        [FirestoreProperty("name")]
        public int Name { get; protected set; }
    }
    
    [FirestoreData]
    public class MapMarkerData : MapMarkerDataBase
    {
        [FirestoreProperty("assetRef")]
        public DocumentReference? AssetRef { get; protected set; } = null!;
    }
    
    public class MapMarkerDataJson : MapMarkerDataBase
    {
        [FirestoreProperty("assetRef")]
        public ReferenceMeta? AssetRef { get; protected set; } = null!;
        
        public MapMarkerDataJson(MapMarkerData marker)
        {
            Id = marker.Id;
            Name = marker.Name;
            if (marker.AssetRef != null)
            {
                AssetRef = new ReferenceMeta(marker.AssetRef.Path, marker.AssetRef.GetType().ToString());
            }
        }
    }
}