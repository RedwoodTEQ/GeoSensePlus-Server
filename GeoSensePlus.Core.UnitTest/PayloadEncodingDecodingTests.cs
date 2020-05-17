using GeoSensePlus.App.AssetTracking.Codecs;
using GeoSensePlus.App.AssetTracking.Messages;
using GeoSensePlus.Core.Codec;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GeoSensePlus.Core.UnitTest
{
    public class PayloadEncodingDecodingTests
    {

        [Fact]
        public void Binary_encoder_encoding()
        {
            Assert.Equal("3B342313", new BinaryEncoder().EncodeNext(0x3b342313, 8).Build());
            Assert.Equal("0008", new BinaryEncoder().EncodeNext(8, 4).Build());
            Assert.Equal("F8", new BinaryEncoder().EncodeNext(-8, 2).Build());
            Assert.Equal("51", new BinaryEncoder().EncodeNext(81, 2).Build());

            BinaryEncoder encoder2 = new BinaryEncoder();
            Assert.Equal("1918FC01F04DF851", encoder2.EncodeNext("1918FC01F04D").EncodeNext(-8, 2).EncodeNext(81, 2).Build());
        }

        [Fact]
        public void Binary_encoder_with_decoder()
        {
            IBinaryDecoder decoder = new BinaryDecoder();
            decoder.ResetPayload("1918FC01F04DF851");
            Assert.Equal("1918FC01F04D", decoder.DecodeNextString(12));
            Assert.Equal(-8, decoder.DecodeNextInt(2));
            Assert.Equal(81, decoder.DecodeNextInt(2));
        }

        [Fact]
        public void Binary_encoder_exception()
        {
            IBinaryEncoder encoder = new BinaryEncoder();
            Assert.Throws<Exception>(() => encoder.EncodeNext(0x3b342313, 9));
            Assert.Throws<Exception>(() => encoder.EncodeNext(0x3b342313, 0));
        }

        [Fact]
        public void Binary_decoder_for_negative_bytes()
        { 
            IBinaryDecoder decoder = new BinaryDecoder();
            decoder.ResetPayload("EADCAD8C9A9EC6BD1EA1B8C10C5BD18A0E2A");
            Assert.Equal(-2, decoder.DecodeNextInt(1));
            Assert.Equal(-83, decoder.DecodeNextInt(2));
            Assert.Equal(-851, decoder.DecodeNextInt(3));
            Assert.Equal(-29542, decoder.DecodeNextInt(4));
            Assert.Equal(-398229, decoder.DecodeNextInt(5));
            Assert.Equal(-3020261, decoder.DecodeNextInt(6));
            Assert.Equal(-121566117, decoder.DecodeNextInt(7));
            Assert.Equal(-779481558, decoder.DecodeNextInt(8));
        }

        [Fact]
        public void Binary_decoder_for_positive_bytes()
        {
            IBinaryDecoder decoder = new BinaryDecoder();
            decoder.ResetPayload("76F3AC3ACB5AB325AB32C15AB32C775AB32C");
            Assert.Equal(7, decoder.DecodeNextInt(1));
            Assert.Equal(111, decoder.DecodeNextInt(2));
            Assert.Equal(940, decoder.DecodeNextInt(3));
            Assert.Equal(15051, decoder.DecodeNextInt(4));
            Assert.Equal(371506, decoder.DecodeNextInt(5));
            Assert.Equal(5944108, decoder.DecodeNextInt(6));
            Assert.Equal(22721324, decoder.DecodeNextInt(7));
            Assert.Equal(2002432812, decoder.DecodeNextInt(8));
        }

        [Fact]
        public void Binary_decoder_for_unsigned_bytes()
        {
            IBinaryDecoder decoder = new BinaryDecoder();
            decoder.ResetPayload("EEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            Assert.Equal((uint)14, decoder.DecodeNextUnsigned(1));
            Assert.Equal((uint)238, decoder.DecodeNextUnsigned(2));
            Assert.Equal((uint)3822, decoder.DecodeNextUnsigned(3));
            Assert.Equal((uint)61166, decoder.DecodeNextUnsigned(4));
            Assert.Equal((uint)978670, decoder.DecodeNextUnsigned(5));
            Assert.Equal((uint)15658734, decoder.DecodeNextUnsigned(6));
            Assert.Equal((uint)250539758, decoder.DecodeNextUnsigned(7));
            Assert.Equal((uint)4008636142, decoder.DecodeNextUnsigned(8));
        }

        [Fact]
        public void Business_object_encoder_with_decoder()
        {
            IPayloadEncoder<List<IndoorTagPayloadInfo>> encoder = new IndoorTagPayloadEncoder();
            IPayloadDecoder<List<IndoorTagPayloadInfo>> decoder = new IndoorTagPayloadDecoder();

            string payloadString = "ff010100820002611918fc01f1ffdac51918fc01f04dd8c5";

            var list = decoder.Decode(payloadString);
            Assert.Equal(2, list.Count);

            string encodedString = encoder.Encode(list).ToLower();
            Assert.Equal(payloadString.Substring(16), encodedString);
        }
    }
}
