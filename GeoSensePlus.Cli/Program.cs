using CoreCmd.CommandExecution;
using Google.Cloud.Firestore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using GeoSensePlus.Firestore;
using NetCoreUtils.Database.MongoDb;

namespace GeoSensePlus.Cli
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new AssemblyCommandExecutor().ExecuteAsync(args, c => c.AddMongoDb(new MongoDbSetting { DatabaseName = "GeoSensePlus" }) );
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddFirestoreServices();
        }
    }
}
