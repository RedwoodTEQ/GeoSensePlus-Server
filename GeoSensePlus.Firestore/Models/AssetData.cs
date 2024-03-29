﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Models
{
    public class AssetDataBase
    {
        [FirestoreDocumentId]
        public string MacAddress { get; set; } = null!; // used as asset ID
        
        [FirestoreProperty, ServerTimestamp]
        public Timestamp LastReportTime { get; set; }

        [FirestoreProperty]
        public int SignalStrength { get; set; }

        [FirestoreProperty]
        public int BatteryLevel { get; set; }

        [FirestoreProperty]
        public string Name { get; protected set; } = null!;

        [FirestoreProperty]
        public string Remarks { get; protected set; } = null!;
    }
    
    [FirestoreData]
    public class AssetData : AssetDataBase
    {
        [FirestoreProperty]
        public DocumentReference EdgeRef { get; set; } = null!;
    }
    
    public class AssetDataJson : AssetDataBase
    {
        public ReferenceMeta EdgeRef { get; }
        
        public AssetDataJson(AssetData asset)
        {
            MacAddress = asset.MacAddress;
            LastReportTime = asset.LastReportTime;
            SignalStrength = asset.SignalStrength;
            BatteryLevel = asset.BatteryLevel;
            Name = asset.Name;
            Remarks = asset.Remarks;
            
            EdgeRef = new ReferenceMeta(asset.EdgeRef.Path, asset.EdgeRef.GetType().ToString());
        }
    }
}
