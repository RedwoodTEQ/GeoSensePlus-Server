using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.Core.MessageProcessing.Interfaces;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories;
using GeoSensePlus.Firestore.Repositories.Common;
using GeoSensePlus.Firestore.Services;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.MessageProcessors;

class IndoorAssetReportProcessor : IMessageProcessor<IndoorAssetReportMessage>
{
    IRepository<EdgeData> _edgeRepo;
    IRepository<AssetData> _assetRepo;

    IAssetService _assetService;
    IFirebaseClient _firebaseClient;

    // _todo_2509010311: temporarily disable firebase client to avoid exception that .gs.config.json is missing
    //public IndoorAssetReportProcessor(IAssetService assetService, IFirebaseClient firebaseClient, IRepository<EdgeData> edgeRepo, IRepository<AssetData> assetRepo)
    //public IndoorAssetReportProcessor()
    //{
    //    _firebaseClient = firebaseClient;
    //    _assetService = assetService;
    //    _edgeRepo = edgeRepo;
    //    _assetRepo = assetRepo;
    //}
    public IndoorAssetReportProcessor()
    { }

    public void Process(IndoorAssetReportMessage message)
    {
        Console.WriteLine("IndoorAssetReportProcessor.Process(..) called");
    }

    // _todo_2509010311: temporarily commented, fix it later
    //public void Process(IndoorAssetReportMessage message)
    //{
    //    // if edge doesn't exist
    //    if (_edgeRepo.RetrieveAsync(message.HardwareSerial).Result == null)
    //    {
    //        _edgeRepo.SetAsync(message.HardwareSerial, new EdgeData()).Wait();
    //        Console.WriteLine($"added new edge ({message.HardwareSerial})");
    //    }

    //    var newEdgeRef = _edgeRepo.GetDocument(message.HardwareSerial);
    //    var batch = _firebaseClient.GetFirestoreDb().StartBatch();

    //    foreach (var payload in message.IndoorTagPayloadInfo)
    //    {
    //        _assetService.UpdateEdgeAsync(batch, new AssetData
    //        {
    //            MacAddress = payload.MacAddress,
    //            LastReportTime = Timestamp.GetCurrentTimestamp(),
    //            BatteryLevel = payload.BatteryLevel,
    //            SignalStrength = payload.Rss,
    //            EdgeRef = newEdgeRef
    //        }).Wait();
    //    }

    //    batch.CommitAsync().Wait();
    //}
}
