using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
//using GeoSensePlus.Protos;
using GeoSensePlus.App.AssetTracking;
using GeoSensePlus.App.ProgressTracking;
using GeoSensePlus.Core;
using GeoSensePlus.Server.Mqtt;
using GeoSensePlus.Firestore;

namespace GeoSensePlus.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // For releasing as a global tool, if the appsettings.json is not at the same directory where
            // the application starts, the logging settings in the appsettings.json won't work
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddFilter("Microsoft", LogLevel.Warning);
                builder.AddFilter("System", LogLevel.Error);
            });

            services.AddControllers();

			//services.AddGrpc();
            services.AddMqtt();

            services.AddGeoSensePlusCore();
            services.AddAssetTracking();
            services.AddProgressTracking();

            services.AddFirestoreServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints => {
            //    endpoints.MapGrpcService<DataHandlerService>();
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
