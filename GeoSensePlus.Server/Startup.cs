using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
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
using GeoSensePlus.Mqtt;
using GeoSensePlus.Firestore;
using System.Security.Cryptography.Xml;
using Swashbuckle.AspNetCore.Swagger;
using Google.Protobuf.WellKnownTypes;
using GeoSensePlus.Server.Options;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using NetCoreUtils.Database.MongoDb;
using GeoSensePlus.Server.Data;
using Westwind.AspNetCore.LiveReload;
using NetCoreUtils.Database.InfluxDb;

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
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
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

            services.AddMongoDb(new MongoDbSetting { DatabaseName = "GeoSensePlus" });
            services.AddInfluxDb(new InfluxDbSetting
            {
                Token = "4R1aL7t1hZolnMQezXQxkhhMGlqYUBy7g5Ue8RQAQ9wHn_XIHJN_2EpFqaYcD9F2wv_lt-kHqP8Ym99c7Gv5pw=="
            });

            services.AddFirestoreServices();
            services.AddSwaggerGen(x => {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "GeoSense+ API", Version = "v1"});
            });
            services.AddLiveReload();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseLiveReload();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            var swaggerOptions = new CustomSwaggerOptions();
            Configuration.GetSection("SwaggerOptions").Bind(swaggerOptions);
            app.UseSwagger(option => {
                option.RouteTemplate = swaggerOptions.JsonRoute;
            });
            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint(swaggerOptions.UiEndpoint, swaggerOptions.Description);
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapControllers();
            });
        }
    }
}
