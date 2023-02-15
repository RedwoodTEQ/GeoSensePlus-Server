using GeoSensePlus.App.AssetTracking.MessageProcessors;
using GeoSensePlus.App.AssetTracking.Messages;
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
        readonly IMessageExecutor<IndoorAssetReportMessage> _processor;

        public IndoorAssetReportHandler(
            IPayloadDecoder<List<IndoorTagPayloadInfo>> payloadDecoder,
            IMessageExecutor<IndoorAssetReportMessage> service
            )
        {
            _payloadDecoder = payloadDecoder;
            _processor = service;
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

        protected override void Execute(IndoorAssetReportMessage message, ChannelContext<string> ctx)
        {
            Console.WriteLine($"Handling IndoorAssetReportMessage message ... Payload info count = {message.IndoorTagPayloadInfo.Count}");
            _processor.Execute(message);
            Console.WriteLine("");
        }

        protected override void OnError(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

}
