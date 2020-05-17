using GeoSensePlus.Firestore.Exceptions;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore.Services
{
    public interface IAssetService
    {
        Task UpdateEdgeAsync(WriteBatch batch, AssetData reportedAssetData);
        Task<bool> UpdateEdgeAsync(WriteBatch batch, DocumentReference existingAssetRef, DocumentReference newEdgeRef);
    }

    public class AssetService : IAssetService
    {
        readonly IRepository<AssetData> _assetRepo;

        public AssetService(IRepository<AssetData> assetRepo)
        {
            _assetRepo = assetRepo;
        }

        public async Task UpdateEdgeAsync(WriteBatch batch, AssetData reportedAssetData)
        {
            var reportedAssetRef = _assetRepo.GetDocument(reportedAssetData.MacAddress);
            DocumentReference reportedEdgeRef = reportedAssetData.EdgeRef;

            if(!await UpdateEdgeAsync(batch, reportedAssetRef, reportedEdgeRef).ConfigureAwait(false))
            {
                // add a new asset document
                batch.Set(reportedAssetRef, reportedAssetData); 

                // add the reported asset's reference to the reported edge
                AddAssetRefToEdge(batch, reportedAssetRef, reportedEdgeRef);
            }
        }

        public async Task<bool> UpdateEdgeAsync(WriteBatch batch, DocumentReference existingAssetRef, DocumentReference reportedEdgeRef)
        {
            Console.WriteLine($"Processing data for asset ({existingAssetRef.Id}):");

            AssetData existingAssetData = await _assetRepo.RetrieveAsync(existingAssetRef).ConfigureAwait(false);
            if (existingAssetData == null)
            {
                Console.WriteLine($"Cannot find asset ({existingAssetRef.Id})");
                return false;
            }

             // 1.1 If an asset's current associate edge is different to the reported one
            if (existingAssetData.EdgeRef != null && !existingAssetData.EdgeRef.Equals(reportedEdgeRef))
            {
                // 1.2 remove asset reference from the old edge
                var edgeSnapshot = await existingAssetData.EdgeRef.GetSnapshotAsync().ConfigureAwait(false);
                if (edgeSnapshot.Exists)
                {
                    RemoveAssetRefFromEdge(batch, existingAssetRef, existingAssetData.EdgeRef);
                }

                // 2. Add asset reference to the new edge 
                //    NOTE:
                //    If the asset ref is removed manually from the reported
                //    edge, the current logic can recover the corrupted data.
                AddAssetRefToEdge(batch, existingAssetRef, reportedEdgeRef);
            }


            // 3. update the existing asset's edge reference (even if the edge
            //    keeps unchanged, the timestamp still needs to be updated)
            batch.Update(existingAssetRef, new Dictionary<string, object> {
                { nameof(AssetData.EdgeRef), reportedEdgeRef },
                { nameof(AssetData.LastReportTime), Timestamp.GetCurrentTimestamp()}
            });

            return true;
        }

        private static void RemoveAssetRefFromEdge(WriteBatch batch, DocumentReference assetRef, DocumentReference edgeRef)
        {
            Console.WriteLine($"Removing asset ({assetRef.Id}) from edge ({edgeRef.Id})");
            batch.Update(edgeRef, new Dictionary<string, object> {
                {nameof(EdgeData.AssetRefs), FieldValue.ArrayRemove(assetRef)},
                {nameof(EdgeData.LastUpdate), Timestamp.GetCurrentTimestamp()}
            });
        }

        private static void AddAssetRefToEdge(WriteBatch batch, DocumentReference assetRef, DocumentReference edgeRef)
        {
            Console.WriteLine($"Associating asset ({assetRef.Id}) to edge ({edgeRef.Id})");
            batch.Update(edgeRef, new Dictionary<string, object> {
                {nameof(EdgeData.AssetRefs), FieldValue.ArrayUnion(assetRef)},
                {nameof(EdgeData.LastUpdate), Timestamp.GetCurrentTimestamp()}
            });
        }
    }
}
