using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.Codec
{
    public interface IPayloadEncoder<T>
    {
        string Encode(T payloads);
    }
}
