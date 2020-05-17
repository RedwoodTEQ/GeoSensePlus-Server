using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Core.Exceptions
{
    public class InvalidPayloadStringLength : Exception
    {
        public string Payload { get; set; }

        public InvalidPayloadStringLength(string message, string payload) : base(message)
        {
            Payload = payload;
        }
    }
}
