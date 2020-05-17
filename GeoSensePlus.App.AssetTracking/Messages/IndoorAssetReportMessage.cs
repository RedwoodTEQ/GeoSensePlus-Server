using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.Messages
{
    /// <summary>
    /// Doc: https://gitlab.com/outdoor-asset-tracking-solution/firmware/wikis/Lora-BLE-device
    /// </summary>
    public class IndoorAssetReportMessage
    {
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// For LoRaWAN: DevEUI of the tracking station device (the "edge device")
        /// </summary>
        public string HardwareSerial { get; set; }

        public List<IndoorTagPayloadInfo> IndoorTagPayloadInfo { get; set; }
    }

}
