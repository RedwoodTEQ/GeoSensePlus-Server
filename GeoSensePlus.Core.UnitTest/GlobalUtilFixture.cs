using GeoSensePlus.App.AssetTracking;
using GeoSensePlus.App.ProgressTracking;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GeoSensePlus.Core.UnitTest
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
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// This method will update ServiceProvide as well.
        /// </summary>
        public T GetServiceNewScope<T>()
        {
            ResetServiceProvider();
            return GetService<T>();
        }

        public void ResetServiceProvider()
        {
            _serviceProvider = _serviceProvider.CreateScope().ServiceProvider;
        }

        public T GetService<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public void Dispose() { }
    }
}
