using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class FloorplanDataBase
    {
        [FirestoreDocumentId]
        public string Id { get; protected set; } = null!;

        [FirestoreProperty("Building")]
        public string Building { get; protected set; } = null!;

        [FirestoreProperty("filePath")]
        public string FilePath {get; protected set;} = null!;

        [FirestoreProperty("floor")]
        public string Floor { get; protected set; } = null!;

        [FirestoreProperty("imageUrl")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1056:Uri properties should not be strings", Justification = "<Pending>")]
        public string ImageUrl { get; protected set; } = null!;

        [FirestoreProperty("name")]
        public string Name { get; protected set; } = null!;
    }

    [FirestoreData]
    public class FloorplanData : FloorplanDataBase { }

    public class FloorplanDataJson : FloorplanDataBase
    {
        public FloorplanDataJson(FloorplanData floorplan)
        {
            Id = floorplan.Id;
            Name = floorplan.Name;
            Building = floorplan.Building;
            FilePath = floorplan.FilePath;
            Floor = floorplan.Floor;
            ImageUrl = floorplan.ImageUrl;
        }
    }
}
