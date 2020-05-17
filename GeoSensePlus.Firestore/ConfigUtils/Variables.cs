using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.ConfigUtils
{
    public static class Variables
    {
        public const string configFile = ".gs.config.json";
        //public const string configPathLinux = "etc";
        public const string configDirectory = "GeoSensePlus";
        public const string reportSessionDir = "report-sessions";

        public const string JSON_TENANT = "tenant";
        public const string JSON_FIREBASE_KEY = "firebase_key";
        public const string JSON_FIREBASE_KEY_REGISTRY = "firebase_key_registry";
    }
}
