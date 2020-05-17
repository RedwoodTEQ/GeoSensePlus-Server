using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GeoSensePlus.Firestore.ConfigUtils;
using System.Runtime.InteropServices;

namespace GeoSensePlus.Server
{
    public class Program
    {
        private static string EnumerateGeoSensePlusFolderFiles()
        {
            StringBuilder sb = new StringBuilder();
            string[] filePaths = Directory.GetFiles(ConfigFileUtil.GetConfigDirPath());
            foreach (var file in filePaths)
                sb.AppendLine(file);
            return sb.ToString();
        }

        public static void Main(string[] args)
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

            Console.WriteLine($"Version: {name.Name} {name.Version} {prod}");
            Console.WriteLine($"System: {osDescription}, {osArchitecture}");
            Console.WriteLine("Read GeoSensePlus directory:");
            Console.WriteLine(EnumerateGeoSensePlusFolderFiles());

            // CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
