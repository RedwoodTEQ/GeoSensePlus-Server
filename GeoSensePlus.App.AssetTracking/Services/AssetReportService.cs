using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories;
using GeoSensePlus.Firestore.Repositories.Common;
using GeoSensePlus.Firestore.Services;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.Services
{
    public interface IAssetReportService
    {
        void Execute(IndoorAssetReportMessage message);
    }

    class AssetReportService : IAssetReportService
    {
        IRepository<EdgeData> _edgeRepo;
        IRepository<AssetData> _assetRepo;

        IAssetService _assetService;
        IFirebaseClient _firebaseClient;

        public AssetReportService(IAssetService assetService, IFirebaseClient firebaseClient,  IRepository<EdgeData> edgeRepo, IRepository<AssetData> assetRepo)
        {
            _firebaseClient = firebaseClient;
            _assetService = assetService;
            _edgeRepo = edgeRepo;
            _assetRepo = assetRepo;
        }

        public void Execute(IndoorAssetReportMessage message)
        {
            // if edge doesn't exist
            if(_edgeRepo.RetrieveAsync(message.HardwareSerial).Result == null)
            {
                _edgeRepo.SetAsync(message.HardwareSerial, new EdgeData()).Wait();
                Console.WriteLine($"added new edge ({message.HardwareSerial})");
            }

            var newEdgeRef = _edgeRepo.GetDocument(message.HardwareSerial);
            var batch = _firebaseClient.GetFirestoreDb().StartBatch();

            foreach(var payload in message.IndoorTagPayloadInfo)
            {
                _assetService.UpdateEdgeAsync(batch, new AssetData
                {
                    MacAddress = payload.MacAddress,
                    LastReportTime = Timestamp.GetCurrentTimestamp(),
                    BatteryLevel = payload.BatteryLevel,
                    SignalStrength = payload.Rss,
                    EdgeRef = newEdgeRef
                }).Wait();
            }

            batch.CommitAsync().Wait();
        }
    }
}
