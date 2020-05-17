using GeoSensePlus.App.AssetTracking;
using GeoSensePlus.App.ProgressTracking;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GeoSensePlus.Core.UnitTest.TestEnv
{
    public class GlobalUtilFixture : IDisposable
    {
        private IServiceProvider _serviceProvider;

        public GlobalUtilFixture()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddAssetTracking();
            serviceCollection.AddProgressTracking();
            serviceCollection.AddGeoSensePlusCore();
            this._serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// This method will update ServiceProvide as well.
        /// </summary>
        public T GetServiceNewScope<T>()
        {
            this.ResetServiceProvider();
            return this.GetService<T>();
        }

        public void ResetServiceProvider()
        {
            this._serviceProvider = this._serviceProvider.CreateScope().ServiceProvider;
        }

        public T GetService<T>()
        {
            return this._serviceProvider.GetService<T>();
        }

        public void Dispose() { }
    }
}
