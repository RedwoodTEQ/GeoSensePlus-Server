using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.Core.Codec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.Codecs
{
    public class IndoorTagPayloadDecoder : IPayloadDecoder<List<IndoorTagPayloadInfo>>
    {
        IBinaryDecoder _decoder = new BinaryDecoder();
        IBinaryStringSegragator _segregator = new BinaryStringSegregator();

        public List<IndoorTagPayloadInfo> Decode(string payloadBinary)
        {
            List<IndoorTagPayloadInfo> result = new List<IndoorTagPayloadInfo>();

            int count = 0;
            _segregator.Segragate(payloadBinary, 16).ForEach(payload =>
            {
                if (count++ == 0)
                    return;

                _decoder.ResetPayload(payload);
                IndoorTagPayloadInfo msg = new IndoorTagPayloadInfo();
                msg.MacAddress = _decoder.DecodeNextString(12);
                msg.Rss = ((int)_decoder.DecodeNextUnsigned(2) - 255);
                msg.BatteryLevel = (int)_decoder.DecodeNextUnsigned(2); // should be between HEX:[0,0x64], i.e. DEC:[0, 100]

                result.Add(msg);
            });

            return result;
        }
    }
}
