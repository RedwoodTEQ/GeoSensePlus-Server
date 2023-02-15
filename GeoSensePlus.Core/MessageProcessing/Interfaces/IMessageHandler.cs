using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.MessageProcessing.Interfaces
{
    public interface IMessageHandler<T>
    {
        bool Handle(T msg, ChannelContext<T> ctx);
    }
}
