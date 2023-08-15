using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class EdgeMarkerDataBase
    {
        [FirestoreDocumentId]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "<Pending>")]
        public string Id { get; protected set; } = null!;

        [FirestoreProperty("location")]
        public ArrayList Location { get; protected set; } = null!;

        [FirestoreProperty("name")] 
        public string Name { get; protected set; } = null!;
    }
    
    [FirestoreData]
    public class EdgeMarkerData : EdgeMarkerDataBase
    {
        public DocumentReference? EdgeRef { get; protected set; }
    }
    
    public class EdgeMarkerDataJson : EdgeMarkerDataBase
    {

        public ReferenceMeta? EdgeRef { get; }

        public EdgeMarkerDataJson(EdgeMarkerData edgeMarker)
        {
            Id = edgeMarker.Id;
            Name = edgeMarker.Name;
            Location = new ArrayList();
            foreach (var value in edgeMarker.Location)
            {
                Location.Add(value);
            }

            if (edgeMarker.EdgeRef != null)
            {
                EdgeRef = new ReferenceMeta(edgeMarker.EdgeRef.Path, edgeMarker.EdgeRef.GetType().ToString());
            }
        }
    }
}
