using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore.Repositories.Common
{
    public interface IRepository<TEntity>
    {
        Task<TEntity> RetrieveAsync(string docId);
        Task SetAsync(string docId, TEntity model);
        Task AddAsync(TEntity model);
        Task<IReadOnlyList<DocumentSnapshot>> ListAllAsync();
        Task RemoveAsync(string docId);
        CollectionReference GetCollection();
        DocumentReference GetDocument(string docId);
        Task UpdateAsync(string docId, IDictionary<string, object> updates);
        Task<TEntity> RetrieveAsync(DocumentReference docRef);
    }
}