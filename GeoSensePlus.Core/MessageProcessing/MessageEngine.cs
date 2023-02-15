using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.MessageProcessing
{
    public interface IMessageEngine
    {
        void Process(byte[] bytes, ChannelContext<byte[]> ctx);
        void Process(string json, ChannelContext<string> ctx);
    }

    public class MessageEngine : IMessageEngine
    {
        readonly IMessageProcessor<string> _jsonProcessor;
        readonly IMessageProcessor<byte[]> _byteProcessor;

        public MessageEngine(
            IMessageProcessor<string> jsonProcessor,
            IMessageProcessor<byte[]> byteProcessor
        )
        {
            _jsonProcessor = jsonProcessor;
            _byteProcessor = byteProcessor;
        }

        public void Process(string json, ChannelContext<string> ctx) => _jsonProcessor.Process(json, ctx);

        public void Process(byte[] bytes, ChannelContext<byte[]> ctx) => _byteProcessor.Process(bytes, ctx);
    }
}
