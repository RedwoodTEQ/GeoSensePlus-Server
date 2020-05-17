using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GeoSensePlus.Core.MessageProcessing
{
    public enum eChannelType
    {
        unknown,
        gRPC,
        http
    }

    public abstract class ChannelContext<TInput>
    {
        //public virtual void WriteAsync();
        public eChannelType ChannelType { get; set; } = eChannelType.unknown;
        public DateTime ReceiveTime { get; set; } = DateTime.Now;
        public TInput RawMessage { get; set; }
        public object MessageObject { get; set; }
    }
}
