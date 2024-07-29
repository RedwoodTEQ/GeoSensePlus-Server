using System;
using System.Collections.Generic;
using System.Text;
using GeoSensePlus.Core.MessageProcessing.Interfaces;

namespace GeoSensePlus.Core.MessageProcessing
{
    public interface IMessageEngine
    {
        void Process(byte[] bytes, ChannelContext<byte[]> ctx);
        void Process(string json, ChannelContext<string> ctx);
    }

    public class MessageEngine : IMessageEngine
    {
        //readonly IMessageProcessor<string> _jsonProcessor;
        //readonly IMessageProcessor<byte[]> _byteProcessor;

        readonly IEnumerable<IMessageHandler<string>> _jsonHandler;
        readonly IEnumerable<IMessageHandler<byte[]>> _byteHandler;

        //public MessageEngine(
        //    IMessageProcessor<string> jsonProcessor,
        //    IMessageProcessor<byte[]> byteProcessor
        //)
        //{
        //    _jsonProcessor = jsonProcessor;
        //    _byteProcessor = byteProcessor;
        //}


        public MessageEngine(
            IEnumerable<IMessageHandler<string>> jsonHandler,
            IEnumerable<IMessageHandler<byte[]>> byteHandler
        )
        {
            _jsonHandler = jsonHandler;
            _byteHandler = byteHandler;
        }


        // TODO: add unit test for the process methods
        public void Process(string json, ChannelContext<string> ctx) => this.ProcessInternal(json, _jsonHandler, ctx);

        public void Process(byte[] bytes, ChannelContext<byte[]> ctx) => this.ProcessInternal(bytes, _byteHandler, ctx);

        private void ProcessInternal<TInput>(TInput msg, IEnumerable<IMessageHandler<TInput>> handlers, ChannelContext<TInput> ctx)
        {
            foreach (var handler in handlers)
            {
                try
                {
                    if (handler.Handle(msg, ctx))
                        break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Message processor error:");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
