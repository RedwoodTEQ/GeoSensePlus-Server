﻿using GeoSensePlus.Firestore.ConfigUtils;
using GeoSensePlus.Firestore.Models;
using GeoSensePlus.Firestore.Repositories.Common;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Firestore.Repositories
{
    public class GeofenceRepository : FirestoreRepositoryBase<GeofenceData>, IRepository<GeofenceData>
    {
        public GeofenceRepository(IFirebaseClient fbClient, IConfigOperator config) : base(fbClient, config) { }

        protected override string SubcollectionName => CollectionName.Geofences;

        //public async Task Add(string docId)
        //{
        //    Console.Write("Geofence.Add() called");
        //    await GetDocument(docId).SetAsync(new Dictionary<string, object>{
        //        { "bounds", "" },
        //        { "centroid", "" },
        //        { "name", "(no name)" },
        //        { "radius", -1 },
        //        { "shapeType", "" },
        //        { "vertices", "[]" },
        //    }).ConfigureAwait(false);
        //}

        //public async Task ListAll()
        //{
        //    var snapshot = await GetCollection().GetSnapshotAsync().ConfigureAwait(false);

        //    foreach (var doc in snapshot.Documents)
        //    {
        //        Console.WriteLine($"Id = {doc.Id}");
        //        Console.WriteLine($"shapeType ={doc.GetValue<string>("shapeType")}");
        //        Console.WriteLine($"bounds = {doc.GetValue<string>("bounds")}\n");
        //    }
        //}
    }

}
