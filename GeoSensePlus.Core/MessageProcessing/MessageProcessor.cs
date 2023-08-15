using System;
using System.Collections.Generic;

namespace GeoSensePlus.Core.MessageProcessing
{
    public interface IMessageProcessor<TInput>
    {
        void Process(TInput msg, ChannelContext<TInput> ctx);
    }

    /// <typeparam name="TInput">Either string (for json) or byte[]</typeparam>
    public class MessageProcessor<TInput> : IMessageProcessor<TInput>
    {
        readonly IEnumerable<IMessageHandler<TInput>> _handlers;

        public MessageProcessor(IEnumerable<IMessageHandler<TInput>> handlers)
        {
            _handlers = handlers;
        }

        public void Process(TInput msg, ChannelContext<TInput> ctx)
        {
            foreach(var handler in _handlers)
            {
                try
                {
                    if (handler.Handle(msg, ctx))
                        break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Message processor error:");
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
