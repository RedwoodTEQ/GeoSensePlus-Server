using Google.Cloud.Firestore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore.Repositories.Common
{
    public interface IManagerRepository
    {
        Task<Dictionary<string, object>> ExportDatabaseDictionary();
        Task<Dictionary<string, object>> ExportDatabaseJsonDictionary();
        Task<bool> ImportJson(string inputPath);
    }
}