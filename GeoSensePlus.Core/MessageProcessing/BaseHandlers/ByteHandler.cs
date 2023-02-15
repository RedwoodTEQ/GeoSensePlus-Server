using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using GeoSensePlus.Core.MessageProcessing.Interfaces;

namespace GeoSensePlus.Core.MessageProcessing.BaseHandlers
{
    public abstract class ByteHandler<TMessage> : IMessageHandler<byte[]>
    {
        public bool Handle(byte[] bytes, ChannelContext<byte[]> ctx)
        {
            try
            {
                ctx.RawMessage = bytes;
                this.Handle(this.Parse(bytes), ctx);
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }

            return false;
        }

        public abstract TMessage Parse(byte[] bytes);
        public abstract void Handle(TMessage message, ChannelContext<byte[]> ctx);
        public virtual void OnError(Exception ex) { }
    }
}
