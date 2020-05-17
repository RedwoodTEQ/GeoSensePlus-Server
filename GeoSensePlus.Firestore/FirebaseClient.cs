using GeoSensePlus.Firestore.ConfigUtils;
using GeoSensePlus.Firestore.Repositories;
using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore
{
    public interface IFirebaseClient
    {
        FirestoreDb GetFirestoreDb();
    }

    public class FirebaseClient : IFirebaseClient
    {
        readonly IConfigOperator _config;

        FirestoreDb _db;

        public FirebaseClient(IConfigOperator config)
        {
            _config = config;
        }

        public FirestoreDb GetFirestoreDb()
        {
            try
            {
                if(_db == null)
                {
                    Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", _config.GetFirebaseKey());
                    _db = FirestoreDb.Create(_config.GetFirebaseProjectId());
                }
                return _db;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
