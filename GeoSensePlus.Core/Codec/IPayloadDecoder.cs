using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.Codec
{
    public interface IPayloadDecoder<T>
    {
        T Decode(string payloadBinary);
    }
}
