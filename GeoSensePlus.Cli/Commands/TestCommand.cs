using GeoSensePlus.Cli.Commands.Shared;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using System;
using Microsoft.Extensions.DependencyInjection;
using GeoSensePlus.Firestore;
using GeoSensePlus.Firestore.Services;

namespace GeoSensePlus.Cli.Commands
{
    public class TestCommand : CommandBase
    { 
        readonly IRepository<AssetData> _repo;

        public TestCommand(IRepository<AssetData> repo)
        {
            _repo = repo;
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
