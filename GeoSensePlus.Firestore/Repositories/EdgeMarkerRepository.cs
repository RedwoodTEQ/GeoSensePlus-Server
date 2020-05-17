using GeoSensePlus.Firestore.ConfigUtils;
using GeoSensePlus.Firestore.Exceptions;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Repositories
{
    public interface IEdgeMarkerRepository : IRepository<EdgeMarkerData>
    {
        string FloorplanId { get; set; }
    }

    public class EdgeMarkerRepository : FirestoreRepositoryBase<EdgeMarkerData>, IEdgeMarkerRepository
    {
        public EdgeMarkerRepository(IFirebaseClient fbClient, IConfigOperator config) : base(fbClient, config) { }

        protected override string SubcollectionName => GetSubcollectionName();

        private string GetSubcollectionName()
        {
            if (string.IsNullOrWhiteSpace(this.FloorplanId))
            {
                const string Message = "Floorplan ID unavailable while getting edge marker collection";
                throw new InvalidDocumentIdException(Message);
            }
            else
                return $"{CollectionName.floorplans}/{this.FloorplanId}/{CollectionName.edgeMarks}";
        }

        public string FloorplanId { get; set; }
    }


}
