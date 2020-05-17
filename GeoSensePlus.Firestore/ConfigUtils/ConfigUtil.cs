using NetCoreUtils.MethodCall;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeoSensePlus.Firestore.ConfigUtils
{
    public interface IConfigUtil
    {
        void ListJsonConfig(string configFile, string registryKey);
        string ReadCreateJsonConfig(string configFile, string key, string unavailableMessage);
        string ReadJsonConfig(string filePath, string key, string unavailableMessage);
        void RegisterJsonConfig(string configFile, string registryKey, string itemKey, string itemValue);
        void SwitchJsonConfig(string configFile, string registryKey, string itemKey, string targetKey);
        string UnregisterJsonConfig(string configFile, string registryKey, string itemKey);
        void WriteJsonConfig(string configFile, string key, string value);
    }

    public class ConfigUtil : IConfigUtil
    {
        static private dynamic GetJson(string filePath)
        {
            return filePath.Forward(File.ReadAllText)
                           .Forward(JsonConvert.DeserializeObject);
        }

        private dynamic GetCreateConfigJson(string configFile)
        {
            if (!File.Exists(configFile))
                File.Create(configFile).Close();

            return GetJson(configFile);
        }

        public void WriteJsonConfig(string configFile, string key, string value)
        {
            dynamic jsonObj = this.GetCreateConfigJson(configFile);
            if (jsonObj == null)
                jsonObj = new Dictionary<string, string>();

            jsonObj[key] = value;

            File.WriteAllText(configFile, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
        }

        public void RegisterJsonConfig(string configFile, string registryKey, string itemKey, string itemValue)
        {
            dynamic jsonObj = this.GetCreateConfigJson(configFile);
            if (jsonObj == null)
                jsonObj = new JObject();

            if (jsonObj[registryKey] == null)
                jsonObj[registryKey] = new JObject();

            jsonObj[registryKey][itemKey] = itemValue;

            File.WriteAllText(configFile, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
        }

        private bool IsJsonKeyAvailable(JObject jsonObj, string registryKey, string itemKey = null)
        {
            if (jsonObj == null)
            {
                Console.WriteLine($"{Variables.configFile} unavailable");
                return false;
            }

            if (jsonObj[registryKey] == null)
            {
                Console.WriteLine($"Registry of '{registryKey}' unavailable");
                return false;
            }

            if (itemKey != null && jsonObj[registryKey][itemKey] == null)
            {
                Console.WriteLine($"Item of {registryKey}.{itemKey} unavailable");
                return false;
            }

            return true;
        }

        public string UnregisterJsonConfig(string configFile, string registryKey, string itemKey)
        {
            string itemValue = null;

            dynamic jsonObj = this.GetCreateConfigJson(configFile);
            if (this.IsJsonKeyAvailable(jsonObj, registryKey, itemKey))
            {
                itemValue = jsonObj[registryKey][itemKey];
                jsonObj[registryKey].Remove(itemKey);
            }

            File.WriteAllText(configFile, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
            return itemValue;
        }

        public void SwitchJsonConfig(string configFile, string registryKey, string itemKey, string targetKey)
        {
            dynamic jsonObj = this.GetCreateConfigJson(configFile);
            if (this.IsJsonKeyAvailable(jsonObj, registryKey, itemKey))
                jsonObj[targetKey] = jsonObj[registryKey][itemKey];

            File.WriteAllText(configFile, JsonConvert.SerializeObject(jsonObj, Formatting.Indented));
        }

        public void ListJsonConfig(string configFile, string registryKey)
        {
            dynamic jsonObj = this.GetCreateConfigJson(configFile);
            if (this.IsJsonKeyAvailable(jsonObj, registryKey))
            {
                var list = jsonObj[registryKey];
                foreach (var item in list)
                    Console.WriteLine(item);
            }
        }

        public string ReadCreateJsonConfig(string configFile, string key, string unavailableMessage)
        {
            string result = null;

            dynamic jsonObj = this.GetCreateConfigJson(configFile);
            if (jsonObj != null)
                result = jsonObj[key];

            if (string.IsNullOrWhiteSpace(result) && !string.IsNullOrWhiteSpace(unavailableMessage))
                Console.WriteLine(unavailableMessage);

            return result;
        }

        public string ReadJsonConfig(string filePath, string key, string unavailableMessage)
        {
            string result = null;

            if (File.Exists(filePath))
            {
                dynamic jsonObj = GetJson(filePath);

                if (jsonObj != null)
                    result = jsonObj[key];

                if (string.IsNullOrWhiteSpace(result))
                    Console.WriteLine(unavailableMessage);
            }

            return result;
        }
    }
}
