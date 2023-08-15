using GeoSensePlus.Firestore.ConfigUtils;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore.Repositories
{
    public class AssetRepository : FirestoreRepositoryBase<AssetData>, IRepository<AssetData>
    {
        public AssetRepository(IFirebaseClient fbClient, IConfigOperator config) : base(fbClient, config) { }

        protected override string SubcollectionName => CollectionName.Assets;
    }
}
