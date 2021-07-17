using GeoSensePlus.App.AssetTracking;
using GeoSensePlus.App.ProgressTracking;
using GeoSensePlus.Core;
using GeoSensePlus.Data;
using GeoSensePlus.Firestore;
using GeoSensePlus.Mqtt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NetCoreUtils.Database;
using NetCoreUtils.Database.InfluxDb;
using NetCoreUtils.Database.MongoDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoSensePlus.WebApi
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

            services.AddControllers();

            //services.AddGrpc();
            services.AddMqtt();

            services.AddGeoSensePlusCore();
            services.AddAssetTracking();
            services.AddProgressTracking();

            services.AddPostgres(Configuration);
            services.AddMongoDb(new MongoDbSetting { DatabaseName = "GeoSensePlus" });
            services.AddInfluxDb(new InfluxDbSetting
            {
                Token = "ky6JucdlHoiCsDta7BxaagInhk33L9D52sK63CzAYB3S5Ptvvxpqdmn133eq-bWQ31uhH06Kkk7mmwFyhFKfuQ=="
            });
            services.AddRepositories<ApplicationDbContext>();

            services.AddFirestoreServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GeoSensePlus.WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeoSensePlus.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
