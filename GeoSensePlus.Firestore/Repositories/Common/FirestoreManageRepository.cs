using GeoSensePlus.Firestore.ConfigUtils;
using Google.Cloud.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoSensePlus.Firestore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GeoSensePlus.Firestore.ExtensionMethods;
using NetCoreUtils.Diagnosis.Logging;


namespace GeoSensePlus.Firestore.Repositories.Common
{
    public class FirestoreManageRepository: IManagerRepository
    {
        private readonly FirestoreDb _db;
        private readonly string _documentsPath;
        
        public FirestoreManageRepository(IFirebaseClient fbClient, IConfigOperator config)
        {
            _db = fbClient.GetFirestoreDb();
            
            // Naming format `projects/{project_id}/databases/{database_id}/documents`
            // So far only default database id is supported by official API, so that it is `(default)`
            _documentsPath = $"projects/{_db.ProjectId}/databases/(default)/documents/";
        }

        #region Import
        
        public async Task<bool> ImportJson(string inputPath)
        {
            
            using (StreamReader file = File.OpenText(@inputPath))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject data = (JObject)JToken.ReadFrom(reader);

                var documentsPath = data["meta"]?["documentsPath"];
                if (documentsPath == null)
                {
                    throw new Exception("Given json file is no validated. Ensure the json file is exported by command `firestore export-json");
                }

                foreach (KeyValuePair<string, JToken> property in data)
                {
                    if(property.Key == "meta") continue;
                    
                    // The property value is empty.
                    if(property.Value is JValue) continue;
                    
                    DocumentReference docRef = _db.Document(property.Key);
                    var docData = ((JObject)property.Value).ToDictionary();
                    var preprocessDocData = ConvertDirectoryPropertiesToFirestoreType(docData);
                    
                    Console.WriteLine($"Importing doc {property.Key}.");
                    
                    await docRef.SetAsync(preprocessDocData);
                }            
            }
            return true;
        }

        Dictionary<string, object> ConvertDirectoryPropertiesToFirestoreType(IDictionary<string, object> dict)
        {
            var result = new Dictionary<string, object>();
            foreach (var property in dict)
            {
                var key = property.Key;
                var value = property.Value;
                
                switch (value)
                {
                    // Process references array
                    case Array array:
                    {
                        result[key] = ConvertArrayObjectsToFirestoreType(array);
                        break;
                    }
                    case Dictionary<string, object> subDict:
                    {
                        if (subDict.ContainsKey("type"))
                        {
                            var typeString = (string) (subDict["type"]);

                            if (typeString! == typeof(DocumentReference).ToString())
                            {
                                result[key] = _db.Document((string) (subDict["path"]!));
                            }
                            else if (typeString! == typeof(CollectionReference).ToString())
                            {
                                result[key] = _db.Collection((string) (subDict["path"]!));
                            }
                            else if (typeString! == typeof(Timestamp).ToString())
                            {
                                if (subDict["value"] is DateTime)
                                {
                                    result[key] = Timestamp.FromDateTime((DateTime) (subDict["value"]));
                                }
                                else
                                {
                                    throw new Exception("Try to convert DateTime data to DateTime data, " +
                                                        "but source data is not DateTime. " +
                                                        $"Source data type {subDict["value"].GetType()}");
                                }
                            }
                            else
                            {
                                throw new Exception($"Unsupported type {typeString}.");
                            }
                        }
                        else
                        {
                            result[key] = ConvertDirectoryPropertiesToFirestoreType(subDict);
                        }

                        break;
                    }
                    default:
                        // If it doesn't match above conditions, it doesn't need to be converted.
                        result[key] = value;
                        break;
                }
            }

            return result;
        }

        List<object> ConvertArrayObjectsToFirestoreType(Array array)
        {
            var convertedValue = new List<object>();
            foreach (var obj in array)
            {
                Debug.Assert(obj != null, nameof(obj) + " != null");
                if (obj is IDictionary<string, object> objDict)
                {
                    if (objDict.ContainsKey("type"))
                    {
                        var typeString = (string) (objDict["type"])!;
                        if (typeString! == typeof(DocumentReference).ToString())
                        {
                            convertedValue.Add(_db.Document((string) (objDict["path"]!)));
                        }
                        else if (typeString! == typeof(CollectionReference).ToString())
                        {
                            convertedValue.Add(_db.Collection((string) (objDict["path"]!)));
                        }
                        else
                        {
                            throw new Exception($"Unsupported type {typeString}");
                        }
                    }
                    else
                    {
                        convertedValue.Add(ConvertDirectoryPropertiesToFirestoreType(objDict));
                    }
                }
                else
                {
                    convertedValue.Add(obj);
                }
            }

            return convertedValue;
        }
        
        #endregion
        
        #region Export

        /// <summary>
        /// Fetch firebase data, return JSON serializable dictionary.
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> ExportDatabaseJsonDictionary()
        {
            var dictionary = await ExportDatabaseDictionary();
            return ConvertToJsonDictionary(dictionary);
        }
        
        /// <summary>
        /// Fetch firebase data, return dictionary of all documents.
        /// </summary>
        /// <returns></returns>
        public async Task<Dictionary<string, object>> ExportDatabaseDictionary()
        {
            var result = new Dictionary<string, object>()
            {
                {"meta", new
                {
                    documentsPath = _documentsPath,
                    data = DateTime.Now,
                }}
            };

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nExport start.");
            Console.WriteLine($"Documents path {_documentsPath}");
            Console.ResetColor();
            
            var collections = _db.ListRootCollectionsAsync();
            await foreach (var collection in collections)
            {
                var documentsDictionary = ExportProcessCollection(collection).Result;
                MergeDictionary<string, object>(ref result, documentsDictionary);
            }

            return result;
        }

        async Task<Dictionary<string, object>> ExportProcessCollection(CollectionReference collectionRef)
        {
            var path = GetRelativePath(collectionRef.Path);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nProcessing collection: {0}", path);
            Console.ResetColor();
            var result = new Dictionary<string, object>();

            var documents = _db.Collection(path).ListDocumentsAsync();

            await foreach (var document in documents)
            {
                var documentsDictionary = await ExportProcessDocument(document.Path, null);
                MergeDictionary<string, object>(ref result, documentsDictionary);
            }

            return result;
        }
        
        async Task<Dictionary<string, object>> ExportProcessDocument(string documentPath, DocumentSnapshot? snapshot)
        {
            var path = GetRelativePath(documentPath);
            Console.WriteLine("\tProcessing document: {0}", path);
            var result = new Dictionary<string, object>();
            
            snapshot ??= _db.Document(path).GetSnapshotAsync().Result;
            result.Add(path, snapshot.ToDictionary());

            var documentRef = _db.Document(path);
            
            IAsyncEnumerable<CollectionReference> subcollections = documentRef.ListCollectionsAsync();
            IAsyncEnumerator<CollectionReference> subcollectionsEnumerator = subcollections.GetAsyncEnumerator(default);
            while (await subcollectionsEnumerator.MoveNextAsync())
            {
                CollectionReference subcollectionRef = subcollectionsEnumerator.Current;
                Console.WriteLine("\n\tFound subcollection with: {0}", subcollectionRef.Path);
                var documentsDictionary = ExportProcessCollection(subcollectionRef).Result;
                MergeDictionary<string, object>(ref result, documentsDictionary);
            }

            return result;
        }
        
        async Task<Dictionary<string, object>> ExportProcessDocument(DocumentReference documentRef, DocumentSnapshot? snapshot)
        {
            var path = GetRelativePath(documentRef.Path);
            Console.WriteLine("\tProcessing document: {0}", path);
            var result = new Dictionary<string, object>();
            
            snapshot ??= _db.Document(path).GetSnapshotAsync().Result;
            result.Add(path, snapshot.ToDictionary());
            
            IAsyncEnumerable<CollectionReference> subcollections = documentRef.ListCollectionsAsync();
            IAsyncEnumerator<CollectionReference> subcollectionsEnumerator = subcollections.GetAsyncEnumerator(default);
            while (await subcollectionsEnumerator.MoveNextAsync())
            {
                CollectionReference subcollectionRef = subcollectionsEnumerator.Current;
                Console.WriteLine("\n\tFound subcollection with: {0}", subcollectionRef.Path);
                var documentsDictionary = ExportProcessCollection(subcollectionRef).Result;
                MergeDictionary<string, object>(ref result, documentsDictionary);
            }

            return result;
        }

        ReferenceMeta ConvertFirestoreReferenceForJson(DocumentReference reference)
        {
            return new ReferenceMeta(GetRelativePath(reference!.Path), reference.GetType().ToString());
        }
        
        ReferenceMeta ConvertFirestoreReferenceForJson(CollectionReference reference)
        {
            return new ReferenceMeta(GetRelativePath(reference.Path), reference.GetType().ToString());
        }

        List<object> ConvertFirestoreReferenceInListForJson(List<object> list)
        {
            var outputArrayList = new List<object>();
            foreach (var item in list)
            {
                if (item!.GetType() == typeof(string))
                {
                    outputArrayList.Add(item);
                }
                else if (item!.GetType() == typeof(List<object>))
                {
                    outputArrayList.Add(ConvertFirestoreReferenceInListForJson(list));
                }
                else if (item is DocumentReference documentRef)
                {
                    outputArrayList.Add(new ReferenceMeta(GetRelativePath(documentRef!.Path), documentRef.GetType().ToString()));
                }
                else if (item is CollectionReference collectionRef)
                {
                    outputArrayList.Add(new ReferenceMeta(GetRelativePath(collectionRef!.Path), collectionRef.GetType().ToString()));
                }
                else if (item is Dictionary<string, object> dictionary)
                {
                    outputArrayList.Add(ConvertToJsonDictionary(dictionary));
                }
                else
                {
                    outputArrayList.Add(item);
                }
            }

            return outputArrayList;
        }
        
        /// <summary>
        /// Convert some DocumentReference data and CollectionReference data to ReferenceMeta data,
        /// which is JSON serializable type.
        /// </summary>
        /// <param name="dict">The dictionary's value is deserialized by `DocumentSnapshot.ToDictionary`</param>
        /// <returns></returns>
        public virtual Dictionary<string, object> ConvertToJsonDictionary(Dictionary<string, object> dict)
        {
            var processedDict = new Dictionary<string, object>();
            foreach (var (key, value) in dict)
            {
                if (value is DocumentReference documentRef)
                {
                    processedDict.Add(key, ConvertFirestoreReferenceForJson(documentRef));
                }
                else if (value is CollectionReference collectionReference)
                {
                    processedDict.Add(key, ConvertFirestoreReferenceForJson(collectionReference));
                }
                else if (value is List<object> list)
                {
                    processedDict.Add(key, ConvertFirestoreReferenceInListForJson(list));
                }
                else if (value is Timestamp time)
                {
                    processedDict.Add(key, new
                    {
                        type = time.GetType().ToString(),
                        value = time.ToDateTime(),
                    });
                }
                else if (value is Dictionary<string, object> subDict)
                {
                    processedDict.Add(key, ConvertToJsonDictionary(subDict));
                }
                else
                {
                    processedDict.Add(key, value);
                }
            }
        
            return processedDict;
        }
        
        #endregion
        
        public void MergeDictionary<T, TF>(ref Dictionary<T, TF> target, Dictionary<T, TF> source)
        {
                foreach (var kvp in source)
                {
                    target.Add(kvp.Key, kvp.Value); 
                }
        }

        /// <summary>
        /// Remove documents root path from given complete path, so that it could be used to fetch data by `_db.Collection(path)`, or `_db.Document(path)`.
        /// </summary>
        /// <param name="completePath">Complete path including project id and database id</param>
        /// <returns>Return new string if it contains db documents root path, otherwise return the original path.</returns>
        public string GetRelativePath(string completePath)
        {
            var array = completePath.Split(_documentsPath);
            if (array.Length == 2)
            {
                return array[1];
            }
            else
            {
                return completePath;
            }
        }
    }
}