using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GeoSensePlus.Firestore.ConfigUtils;
using System.Runtime.InteropServices;

namespace GeoSensePlus.Server
{
    public class SystemInfo
    {
        //private static string EnumerateGeoSensePlusFolderFiles()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string[] filePaths = Directory.GetFiles(ConfigFileUtil.GetConfigDirPath());
        //    foreach (var file in filePaths)
        //        sb.AppendLine(file);
        //    return sb.ToString();
        //}

        public string GetInfo()
        {
            var name = System.Reflection.Assembly.GetEntryAssembly().GetName();
            string osDescription = RuntimeInformation.OSDescription;
            string osArchitecture = RuntimeInformation.OSArchitecture.ToString();

            string prod;
#if DEBUG
            prod = "Development";
#else
            prod = "Production";
#endif

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Version: {name.Name} {name.Version}, {prod}");
            sb.AppendLine($"System: {osDescription}, {osArchitecture}");
            sb.AppendLine($"Configuration File: {ConfigFileUtil.GetConfigFile()}");
            //sb.AppendLine("Read GeoSensePlus directory:");
            //sb.AppendLine(EnumerateGeoSensePlusFolderFiles());

            return sb.ToString();
        }
    }
}
