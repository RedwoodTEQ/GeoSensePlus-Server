using GeoSensePlus.Firestore.ConfigUtils;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories;
using GeoSensePlus.Firestore.Repositories.Common;
using GeoSensePlus.Firestore.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore
{
    static public class ServiceExt
    {
        static public void AddFirestoreServices(this IServiceCollection services)
        {
            services.AddTransient<IRepository<AssetData>, AssetRepository>();
            services.AddTransient<IRepository<EdgeData>, EdgeRepository>();
            services.AddTransient<IRepository<GeofenceData>, GeofenceRepository>();
            services.AddTransient<IRepository<FloorplanData>, FloorplanRepository>();

            services.AddTransient<IEdgeMarkerRepository, EdgeMarkerRepository>();
            services.AddTransient<ITenantRepository, TenantRepository>();

            services.AddTransient<IAssetService, AssetService>();

            services.AddTransient<IConfigOperator, ConfigOperator>();
            services.AddTransient<IConfigUtil, ConfigUtil>();

            services.AddScoped<IFirebaseClient, FirebaseClient>();
        }
    }
}
