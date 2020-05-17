using GeoSensePlus.Firestore.ConfigUtils;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Repositories
{
    public class FloorplanRepository : FirestoreRepositoryBase<FloorplanData>, IRepository<FloorplanData>
    {
        public FloorplanRepository(IFirebaseClient fbClient, IConfigOperator config) : base(fbClient, config) { }

        protected override string SubcollectionName => CollectionName.floorplans;
    }
}
