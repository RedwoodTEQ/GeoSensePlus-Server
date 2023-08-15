using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class IndoorAssetTagDataBase
    {
        [FirestoreDocumentId]
        public string Id { get; protected set; } = null!;
        
        [FirestoreProperty("edge")]
        public string EdgeMarker { get; protected set; } = null!;

        [FirestoreProperty("floor")]
        public int Floor { get; protected set; }

        [FirestoreProperty("name")]
        public int Name { get; protected set; }

        [FirestoreProperty("signal")]
        public string Signal { get; protected set; } = null!;
    }
    
    [FirestoreData]
    public class IndoorAssetTagData : IndoorAssetTagDataBase
    {
        [FirestoreProperty("edgeRef")]
        public DocumentReference? EdgeMarkerRef { get; protected set; } = null!;
    }
    
    public class IndoorAssetTagDataJson : IndoorAssetTagDataBase
    {
        public ReferenceMeta? EdgeRef { get; protected set; } = null!;
        
        public IndoorAssetTagDataJson(IndoorAssetTagData assetTag)
        {
            Id = assetTag.Id;
            Name = assetTag.Name;
            Floor = assetTag.Floor;
            Signal = assetTag.Signal;
            EdgeMarker = assetTag.EdgeMarker;

            if (assetTag.EdgeMarkerRef != null)
            {
                EdgeRef = new ReferenceMeta(assetTag.EdgeMarkerRef.Path, assetTag.EdgeMarkerRef.GetType().ToString());
            }
        }
    }
}