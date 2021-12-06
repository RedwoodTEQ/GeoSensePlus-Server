using NetCoreUtils.MethodCall;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeoSensePlus.Firestore.ConfigUtils
{
    public interface IConfigOperator
    {
        string GetFirebaseKey();
        string GetFirebaseProjectId();
        string GetTenant();
        void ListFirebaseKey();
        void DisplayActivateFirebaseKey();
        void RegisterFirebaseKey(string name, string keyFilePath);
        void SetFirebaseKey(string keyFilePath);
        void SetTenant(string tenantName);
        void SwitchFirebaseKey(string name);
        void UnregisterFirebaseKey(string name);
    }

    public class ConfigOperator : IConfigOperator
    {
        readonly string _configDirPath;
        readonly string _configFile;

        readonly IConfigUtil _configUtil;

        public ConfigOperator(IConfigUtil configUtil)
        {
            _configUtil = configUtil;
            _configDirPath = ConfigFileUtil.GetConfigDirPath();
            _configFile = ConfigFileUtil.GetConfigFile();
        }

        public string GetFirebaseProjectId()
        {
            string keyFile = this.GetFirebaseKey();
            Console.WriteLine($"...DEBUG: firebase key full path: {keyFile}");

            var projId =  _configUtil.ReadJsonConfig(keyFile, "project_id", $"Not able to read firebase project ID in file: {keyFile}");
            Console.WriteLine($"...DEBUG: project ID: {projId}");

            var credEnvValue = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
            Console.WriteLine($"...DEBUG: GOOGLE_APPLICATION_CREDENTIALS = {credEnvValue}");

            return projId;
        }

        private void CopyKeyFile(string keyFileName)
        {
            if (!Directory.Exists(_configDirPath))
            {
                Directory.CreateDirectory(_configDirPath);
            }

            if (!File.Exists(keyFileName))
            {
                throw new FileNotFoundException($"Error: specified file does not exist: ${keyFileName}");
            }

            string targetFile = Path.GetFullPath($"{_configDirPath}/{keyFileName}");    // normalize the full path string (for '/' and '\' symbols)

            // copy key file to config directory
            Console.WriteLine($"Copying {keyFileName} to {targetFile} ...");
            File.Copy(keyFileName, targetFile, true);   // true: overwrite
        }

        public void SetFirebaseKey(string keyFileName)
        {
            CopyKeyFile(keyFileName);
            _configUtil.WriteJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY, keyFileName);
        }

        /// <summary>
        /// Get the firebase key file's full path.
        /// </summary>
        public string GetFirebaseKey()
        {
            string keyFileName = _configUtil.ReadCreateJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY, "Warning: the firebase key info is not available");
            return Path.GetFullPath($"{_configDirPath}/{keyFileName}");     // normalize the full path string (for '/' and '\' symbols)
        }

        public void RegisterFirebaseKey(string name, string keyFileName)
        {
            CopyKeyFile(keyFileName);
            _configUtil.RegisterJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY_REGISTRY, name, keyFileName);
        }

        public void UnregisterFirebaseKey(string name)
        {
            string keyFileName = _configUtil.UnregisterJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY_REGISTRY, name);
            File.Delete($"{_configDirPath}/{keyFileName}");
        }

        public void SwitchFirebaseKey(string name)
        {
            _configUtil.SwitchJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY_REGISTRY, name, Variables.JSON_FIREBASE_KEY);
        }

        public void SetTenant(string tenantName)
        {
            _configUtil.WriteJsonConfig(_configFile, Variables.JSON_TENANT, tenantName);
        }

        public void ListFirebaseKey()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Registered firebase keys:");
            _configUtil.ListJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY_REGISTRY);
            Console.ResetColor();
        }
        public void DisplayActivateFirebaseKey()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\nActivate firebase key:");
            _configUtil.ListJsonConfig(_configFile, Variables.JSON_FIREBASE_KEY);
            Console.ResetColor();
        }

        public string GetTenant()
        {
            return _configUtil.ReadCreateJsonConfig(_configFile, Variables.JSON_TENANT, "");
        }
    }
}
