using GeoSensePlus.Firestore.ConfigUtils;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore.Repositories.Common
{
    abstract public class FirestoreRepositoryBase<T> : IRepository<T>
    {
        FirestoreDb _db;
        string _tenantPath;

        protected abstract string SubcollectionName { get; }
        protected FirestoreDb DB { get { return _db; } }

        public FirestoreRepositoryBase(IFirebaseClient fbClient, IConfigOperator config)
        {
            _db = fbClient.GetFirestoreDb();

            string tenantName = config.GetTenant();
            _tenantPath = String.IsNullOrWhiteSpace(tenantName)?"":$"tenants/{tenantName}/";
        }

        #region general basic firestore operations
        public CollectionReference GetCollection()
        {
            return _db.Collection($"{_tenantPath}{SubcollectionName}");
        }

        public DocumentReference GetDocument(string docId)
        {
            return _db.Document($"{_tenantPath}{SubcollectionName}/{docId}");
        }
        #endregion

        #region model relevant operations
        public virtual async Task<T> RetrieveAsync(string docId)
        {
            return await RetrieveAsync(GetDocument(docId)).ConfigureAwait(false);
        }

        public virtual async Task<T> RetrieveAsync(DocumentReference docRef)
        {
            var snapshot = await docRef.GetSnapshotAsync().ConfigureAwait(false);
            if(snapshot != null)
                return snapshot.ConvertTo<T>();
            else
                return default(T);
        }

        public virtual async Task AddAsync(T entity)
        {
            await GetCollection().AddAsync(entity).ConfigureAwait(false);
        }

        public virtual async Task SetAsync(string docId, T entity)
        {
            await GetDocument(docId).SetAsync(entity).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(string docId, IDictionary<string, object> updates)
        {
            await GetDocument(docId).UpdateAsync(updates).ConfigureAwait(false);
        }

        public virtual async Task<IReadOnlyList<DocumentSnapshot>> ListAllAsync()
        {
            var snapshot = await GetCollection().GetSnapshotAsync().ConfigureAwait(false);
            return snapshot.Documents;
        }

        public virtual async Task RemoveAsync(string docId)
        {
            await GetDocument(docId).DeleteAsync().ConfigureAwait(false);
        }

        #endregion
    }
}
