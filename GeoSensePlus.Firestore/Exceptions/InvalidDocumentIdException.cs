using System;
using System.Collections.Generic;
using System.Text;

namespace GeoSensePlus.Firestore.Exceptions
{
    public class InvalidDocumentIdException : Exception
    {
        public InvalidDocumentIdException(string message) : base(message) { }

        public InvalidDocumentIdException(string message, Exception innerException) : base(message, innerException) { }

        public InvalidDocumentIdException() { }
    }
}
