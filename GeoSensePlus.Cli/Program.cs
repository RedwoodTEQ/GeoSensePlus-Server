using CoreCmd.CommandExecution;
using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using GeoSensePlus.Firestore;

namespace GeoSensePlus.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            new AssemblyCommandExecutor().Execute(args);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddFirestoreServices();
        }
    }
}
