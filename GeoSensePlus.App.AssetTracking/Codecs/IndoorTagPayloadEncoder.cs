using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.Core.Codec;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.Codecs
{
    public class IndoorTagPayloadEncoder : IPayloadEncoder<List<IndoorTagPayloadInfo>>
    {
        BinaryEncoder _encoder = new BinaryEncoder();

        public string Encode(List<IndoorTagPayloadInfo> payloads)
        {
            StringBuilder result = new StringBuilder();
            foreach(var payload in payloads)
            {
                _encoder.EncodeNext(payload.MacAddress);
                _encoder.EncodeNext(payload.Rss, 2);
                _encoder.EncodeNext(payload.BatteryLevel, 2);
                result.Append(_encoder.Build());
                _encoder.Reset();
            }
            return result.ToString();
        }
    }
}
