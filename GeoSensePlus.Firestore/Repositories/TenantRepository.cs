using GeoSensePlus.Firestore.Repositories.Common;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using NetCoreUtils.MethodCall;

namespace GeoSensePlus.Firestore.Repositories
{
    public interface ITenantRepository
    {
        Task Add(string docId);
        Task ListAll();
        Task Remove(string docId);
    }

    public class TenantRepository : ITenantRepository
    {
        readonly CollectionReference _col;

        public TenantRepository(IFirebaseClient fbClient)
        {
            _col = fbClient.GetFirestoreDb().Collection("tenants");
        }

        public Task Add(string docId)
        {
            throw new NotImplementedException();
        }

        public Task ListAll()
        {
            throw new NotImplementedException();
        }

        public Task Remove(string docId)
        {
            throw new NotImplementedException();
        }
    }
}
