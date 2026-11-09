using GeoSensePlus.App.AssetTracking;
using GeoSensePlus.App.ProgressTracking;
using GeoSensePlus.Core.Services;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using GeoSensePlus.Firestore.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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

            /** Mocks for firebase/firestore, might be obsolete, but keeping it for now. **/
            //sc.AddSingleton(new Mock<IAssetService>().Object);
            //sc.AddSingleton(new Mock<IFirebaseClient>().Object);
            //sc.AddSingleton(new Mock<IRepository<EdgeData>>().Object);
            //sc.AddSingleton(new Mock<IRepository<AssetData>>().Object);

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
