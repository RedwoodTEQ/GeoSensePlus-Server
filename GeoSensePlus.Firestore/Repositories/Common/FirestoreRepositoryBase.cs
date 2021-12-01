using GeoSensePlus.Firestore.ConfigUtils;
using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeoSensePlus.Firestore.Models;

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
        
        public virtual async Task<Dictionary<string, object>> RetrieveDictionaryAsync(string docId)
        {
            return await RetrieveDictionaryAsync(GetDocument(docId)).ConfigureAwait(false);
        }
        
        public virtual async Task<Dictionary<string, object>> RetrieveDictionaryAsync(DocumentReference docRef)
        {
            var snapshot = await docRef.GetSnapshotAsync().ConfigureAwait(false);
            return snapshot?.ToDictionary()!;
        } 
        
        public virtual async Task<Dictionary<string, object>> RetrieveJsonDictionaryAsync(string docId)
        {
            return await RetrieveJsonDictionaryAsync(GetDocument(docId)).ConfigureAwait(false);
        }
        
        public virtual async Task<Dictionary<string, object>> RetrieveJsonDictionaryAsync(DocumentReference docRef)
        {
            var snapshot = await docRef.GetSnapshotAsync().ConfigureAwait(false);
            var dict = RetrieveDictionaryAsync(docRef);
            return ConvertToJsonDictionary(dict.Result);
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

        /// <summary>
        /// Convert some firestore data type to JSON serializable type.
        /// </summary>
        /// <param name="dict">The dictionary data which is deserialized by `DocumentSnapshot.ToDictionary`</param>
        /// <returns></returns>
        public virtual Dictionary<string, object> ConvertToJsonDictionary(Dictionary<string, object> dict)
        {
            var processedDict = new Dictionary<string, object>();
            foreach (var (key, value) in dict)
            {
                if (value is DocumentReference documentRef)
                {
                    processedDict.Add(key, ProcessReference(documentRef));
                }
                else if (value is List<object> list)
                {
                    processedDict.Add(key, ProcessList(list));
                }
                else if (value is Timestamp time)
                {
                    processedDict.Add(key, new
                    {
                        type = time.GetType().ToString(),
                        value = time.ToDateTime(),
                    });
                }
                else
                {
                    processedDict.Add(key, value);
                }
            }

            return processedDict;
        }

        public virtual ReferenceMeta ProcessReference(DocumentReference reference)
        {
            return new ReferenceMeta(reference!.Path, reference.GetType().ToString());
        }
        
        public virtual ReferenceMeta ProcessReference(CollectionReference reference)
        {
            return new ReferenceMeta(reference.Path, reference.GetType().ToString());
        }

        public virtual List<object> ProcessList(List<object> list)
        {
            var outputArrayList = new List<object>();
            foreach (var item in list)
            {
                if (item!.GetType() == typeof(ArrayList))
                {
                    outputArrayList.Add(ProcessList(list));
                }
                else if (item is DocumentReference temp)
                {
                    outputArrayList.Add(new ReferenceMeta(temp!.Path, temp.GetType().ToString()));
                }
            }

            return outputArrayList;
        }

        #endregion
    }
}
