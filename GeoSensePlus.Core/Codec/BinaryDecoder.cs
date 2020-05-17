using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GeoSensePlus.Core.Codec
{
    public interface IBinaryDecoder
    {
        int DecodeNextInt(int steps);
        string DecodeNextString(int steps);
        uint DecodeNextUnsigned(int steps);
        void ResetPayload(string payload);
        void ResetStep();
    }

    public class BinaryDecoder : IBinaryDecoder
    {
        int _cursor = 0;

        string _payload;

        public BinaryDecoder() { }

        public BinaryDecoder(string payload)
        {
            _payload = payload;
        }

        public void ResetPayload(string payload)
        {
            _payload = payload;
            _cursor = 0;
        }

        public void ResetStep()
        {
            _cursor = 0;
        }

        private uint DecodeNextUnsignedNoStepping(int steps)
        {
            if(steps < 1 || steps > 8)
                throw new Exception("Steps must be within the range of [1,8]");

            string byteStr = _payload.Substring(_cursor, steps);
            return Convert.ToUInt32(byteStr, 16);
        }

        public uint DecodeNextUnsigned(int steps)
        {
            var result = DecodeNextUnsignedNoStepping(steps);
            _cursor += steps;
            return result;
        }

        public int DecodeNextInt(int steps)
        {
            int result = (int) DecodeNextUnsignedNoStepping(steps);
            unchecked
            {
                switch (steps)
                {
                    case 1:     // negative integer of half byte
                        result = (result <= 0x7) ? result : result | (int)0xfffffff0;
                        break;
                    case 2:     // negative integer of 1 byte
                        result = (result <= 0x7F) ? result : result | (int)0xffffff00;
                        break;
                    case 3:     // negative integer of 1.5 bytes
                        result = (result <= 0x7FF) ? result : result | (int)0xfffff000;
                        break;
                    case 4:     // negative integer of 2 bytes
                        result = (result <= 0x7FFF) ? result : result | (int)0xffff0000;
                        break;
                    case 5:     // negative integer of 2.5 bytes
                        result = (result <= 0x7FFFF) ? result : result | (int)0xfff00000;
                        break;
                    case 6:     // negative integer of 3 bytes
                        result = (result <= 0x7FFFFF) ? result : result | (int)0xff000000;
                        break;
                    case 7:     // negative integer of 3.5 bytes
                        result = (result <= 0x7FFFFFF) ? result : result | (int)0xf0000000;
                        break;
                }
            }
            _cursor += steps;
            return result;
        }

        public string DecodeNextString(int steps)
        {
            string result = _payload.Substring(_cursor, steps).ToUpper();
            _cursor += steps;
            return result;
        }
    }


}
