using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Services;

namespace GeoSensePlus.Cli.Commands
{
    public class AssetCommand : CommandBase
    {
        readonly IRepository<AssetData> _repo;

        public AssetCommand()
        {
            _repo = sp.GetService<IRepository<AssetData>>();
        }

        public void Display(string docId)
        {
            this.Execute(() => {
                var obj = _repo.RetrieveAsync(docId).Result;
                Console.WriteLine($"MAC Address     : {obj.MacAddress}");
                Console.WriteLine($"Report Time     : {obj.LastReportTime}");
                Console.WriteLine($"Signal Strength : {obj.SignalStrength}");
                Console.WriteLine($"Battery Level   : {obj.BatteryLevel}");
                Console.WriteLine($"Edge Reference  : {obj.EdgeRef.Id}");
            });
        }

        public void GetEdge(string assetId)
        {
            this.Execute(() => {
                var asset = _repo.RetrieveAsync(assetId).Result;
                Console.WriteLine(asset.EdgeRef.Id);
            });
        }

        public void SetEdge(string assetId, string targetEdgeId)
        {
            this.Execute(() => {
                var assetSvc = sp.GetService<IAssetService>();
                var edgeRepo = sp.GetService<IRepository<EdgeData>>();

                var batch = sp.GetService<IFirebaseClient>().GetFirestoreDb().StartBatch();
                assetSvc.UpdateEdgeAsync(batch, _repo.GetDocument(assetId), edgeRepo.GetDocument(targetEdgeId)).Wait();
                batch.CommitAsync().Wait();
            });
        }

        /// <summary>
        /// Attach asset between 2 edges to mimic moving asset between 2 edges
        /// </summary>
        public void WaggleEdges(string assetId, string edgeId1, string edgeId2)
        {
            this.Execute(() => {
                var assetSvc = sp.GetService<IAssetService>();
                var edgeRepo = sp.GetService<IRepository<EdgeData>>();

                string targetEdgeId = edgeId1;
                var batch = sp.GetService<IFirebaseClient>().GetFirestoreDb().StartBatch();
                do
                {
                    Console.WriteLine($"Moving asset ({assetId}) to edge ({targetEdgeId}) ...");
                    assetSvc.UpdateEdgeAsync(batch, _repo.GetDocument(assetId), edgeRepo.GetDocument(targetEdgeId)).Wait();
                    batch.CommitAsync().Wait();
                    targetEdgeId = targetEdgeId == edgeId1 ? edgeId2 : edgeId1;
                    Console.WriteLine("Input 'q' to quit ...");
                }
                while (Console.ReadLine() != "q") ;
            });
        }
    }
}
