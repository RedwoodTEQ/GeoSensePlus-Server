using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.Codec
{
    public interface IBinaryEncoder
    {
        string Build();
        IBinaryEncoder EncodeNext(int value, int charNumber);
        IBinaryEncoder EncodeNext(string value);
        void Reset();
    }

    public class BinaryEncoder : IBinaryEncoder
    {
        StringBuilder _strBuilder = new StringBuilder();

        public void Reset()
        {
            _strBuilder = new StringBuilder();
        }

        public IBinaryEncoder EncodeNext(int value, int charNumber)
        {
            if (charNumber < 1 || charNumber > 8)
                throw new Exception("Character number should be in the range of [1, 8]");

            _strBuilder.Append(value.ToString("X8").Substring(8 - charNumber));
            return this;
        }

        public IBinaryEncoder EncodeNext(string value)
        {
            _strBuilder.Append(value.ToUpper());
            return this;
        }

        public string Build()
        {
            return _strBuilder.ToString();
        }
    }
}
