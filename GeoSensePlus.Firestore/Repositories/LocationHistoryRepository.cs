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
    public interface ILocationHistoryRepository : IRepository<LocationHistoryData>
    {
        string MapMarkerId { get; set; }
    }

    public class LocationHistoryRepository : FirestoreRepositoryBase<LocationHistoryData>, ILocationHistoryRepository
    {
        public LocationHistoryRepository(IFirebaseClient fbClient, IConfigOperator config) : base(fbClient, config) { }

        protected override string SubcollectionName => GetSubcollectionName();

        private string GetSubcollectionName()
        {
            if (string.IsNullOrWhiteSpace(this.MapMarkerId))
            {
                const string Message = "MapMarker ID unavailable while getting map marker collection";
                throw new InvalidDocumentIdException(Message);
            }
            else
                return $"{CollectionName.MapMarkers}/{this.MapMarkerId}/{CollectionName.LocationHistory}";
        }

        public string MapMarkerId { get; set; }
    }
}