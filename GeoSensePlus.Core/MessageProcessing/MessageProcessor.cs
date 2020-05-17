using System;
using System.Collections.Generic;

namespace GeoSensePlus.Core.MessageProcessing
{
    public interface IMessageProcessor<T>
    {
        void Process(T msg, ChannelContext<T> ctx);
    }

    /// <typeparam name="T">Common, generic types, like json string, binary array, xml string etc.</typeparam>
    public class MessageProcessor<T> : IMessageProcessor<T>
    {
        readonly IEnumerable<IMessageHandler<T>> _handlers;

        public MessageProcessor(IEnumerable<IMessageHandler<T>> handlers)
        {
            _handlers = handlers;
        }

        public void Process(T msg, ChannelContext<T> ctx)
        {
            foreach(var handler in _handlers)
            {
                if(this.Invoke(() => { return handler.Handle(msg, ctx); })) // i.e. only allow one handler to make valid process
                    break;
            }
        }

        private bool Invoke(Func<bool> fn)
        {
            try
            {
                return fn();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Message processor error:");
                Console.WriteLine(ex);
            }

            return false;
        }
    }
}
