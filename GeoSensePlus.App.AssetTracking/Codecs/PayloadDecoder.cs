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
            _segregator.Segregate(payloadBinary, 16).ForEach(payload =>
            {
                if (count++ == 0)   // skip over the 1st segragated substring
                    return;

                _decoder.ResetPayload(payload);
                IndoorTagPayloadInfo msg = new IndoorTagPayloadInfo();
                msg.MacAddress = _decoder.DecodeNextString(12);
                msg.Rss = _decoder.DecodeNextInt(2);
                msg.BatteryLevel = (int)_decoder.DecodeNextUnsigned(2); // should be between HEX:[0,0x64], i.e. DEC:[0, 100]

                result.Add(msg);
            });

            return result;
        }
    }
}
