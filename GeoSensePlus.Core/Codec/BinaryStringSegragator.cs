using GeoSensePlus.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeoSensePlus.Core.Codec
{
    public interface IBinaryStringSegragator
    {
        List<string> Segragate(string binStr, int singlePayloadLength);
    }

    public class BinaryStringSegregator : IBinaryStringSegragator
    {
        // NOTE: throws exception
        public List<string> Segragate(string binStr, int singlePayloadLength)
        {
            if (binStr.Length % singlePayloadLength != 0)
                throw new InvalidPayloadStringLength($"Invalid payload string length {binStr.Length} for: ${binStr}", binStr);
            else
            {
                return Enumerable.Range(0, binStr.Length / singlePayloadLength)
                                 .Select(i => binStr.Substring(i * singlePayloadLength, singlePayloadLength)).ToList();
            }
        }
    }
}
