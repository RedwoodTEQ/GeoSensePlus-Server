using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.App.AssetTracking.Services;
using GeoSensePlus.Core.Codec;
using GeoSensePlus.Core.MessageProcessing;
using GeoSensePlus.Core.MessageProcessing.BaseHandlers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.App.AssetTracking.Handlers
{

    public class IndoorAssetReportHandler : JsonHandler<IndoorAssetReportMessage>
    {
        readonly IPayloadDecoder<List<IndoorTagPayloadInfo>> _payloadDecoder;
        readonly IAssetReportService _service;

        public IndoorAssetReportHandler(
            IPayloadDecoder<List<IndoorTagPayloadInfo>> payloadDecoder,
            IAssetReportService service
            )
        {
            _payloadDecoder = payloadDecoder;
            _service = service;
        }

        protected override IndoorAssetReportMessage Parse(dynamic msg)
        {
            Console.WriteLine("Parsing indoor asset report message ...");

            StringBuilder sb = new StringBuilder();
            foreach(byte b in msg.payload_raw.data)
            {
                sb.Append(b.ToString("x2"));
            }

            string payload = sb.ToString();
            Console.WriteLine($"payload = {payload}");

            return new IndoorAssetReportMessage
            {
                TimeStamp = msg.metadata.time,
                HardwareSerial = msg.hardware_serial,
                IndoorTagPayloadInfo = _payloadDecoder.Decode((string)payload)
            };
        }

        protected override void Handle(IndoorAssetReportMessage message, ChannelContext<string> ctx)
        {
            Console.WriteLine($"Handling IndoorAssetReportMessage message ... Payload info count = {message.IndoorTagPayloadInfo.Count}");
            _service.Execute(message);
            Console.WriteLine("");
        }

        protected override void OnError(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

}
