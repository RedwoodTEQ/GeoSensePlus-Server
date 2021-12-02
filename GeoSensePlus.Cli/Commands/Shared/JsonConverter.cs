using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using GeoSensePlus.Firestore.Models;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GeoSensePlus.Cli.Commands.Shared
{
    /// <summary>
    /// Convert firestore document reference type to string.
    /// </summary>
    public class DocumentReferenceConverter : JsonConverter<DocumentReference>
    {
        public override void WriteJson(JsonWriter writer, DocumentReference value, JsonSerializer serializer)
        {
            var data = new Dictionary<string, object>()
            {
                {"Converted", true},
                { "Path", value.Path},
                { "Type", value.GetType()},
            };
            writer.WriteValue(
                JsonConvert.SerializeObject(data, Formatting.Indented)
            );
        }
        
        public override DocumentReference ReadJson(JsonReader reader, Type objectType, DocumentReference existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    
    /// <summary>
    /// Convert firestore collection reference type to string.
    /// </summary>
    public class CollectionReferenceConverter : JsonConverter<CollectionReference>
    {
        public override void WriteJson(JsonWriter writer, CollectionReference value, JsonSerializer serializer)
        {
            var data = new Dictionary<string, object>()
            {
                {"Converted", true},
                { "Path", value.Path},
                { "Type", value.GetType()},
            };
            writer.WriteValue(
                JsonConvert.SerializeObject(data, Formatting.Indented)
            );
        }
        
        public override CollectionReference ReadJson(JsonReader reader, Type objectType, CollectionReference existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    
}