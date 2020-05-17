using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.Messages
{
    public class IndoorTagPayloadInfo
    {
        public string MacAddress { get; set; }

        /// <summary>
        /// Received signal strength
        /// </summary>
        public int Rss { get; set; }
        public int BatteryLevel { get; set; }
    }
}
