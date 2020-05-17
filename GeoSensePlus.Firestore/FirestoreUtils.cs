using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json;

namespace GeoSensePlus.Firestore
{
    public static class FirestoreUtils
    {
        public static void InspectDocument(DocumentSnapshot snapshot)
        {
            var dict = snapshot.ToDictionary();
            foreach(var (k, v) in dict)
            {
                Console.WriteLine($"({v.GetType().ToString()}) {k} = {v.ToString()} ");
            }
        }

        public static void PrintObject<T>(T obj)
        {
            Console.WriteLine(JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions { WriteIndented = true }));
        }

        public static string GetDocumentRelativePath(DocumentReference docRef)
        {
            const string documents = "/documents/";
            int index = docRef.Path.IndexOf(documents) + documents.Length;
            return docRef.Path.Substring(index);
        }
    }
}
