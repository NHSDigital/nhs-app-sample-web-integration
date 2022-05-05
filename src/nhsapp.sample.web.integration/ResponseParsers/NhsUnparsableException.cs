using System;

namespace nhsapp.sample.web.integration.ResponseParsers
{
    public class NhsUnparsableException : FormatException
    {
        public NhsUnparsableException()
        {
        }

        public NhsUnparsableException(string message) : base(message)
        {
        }

        public NhsUnparsableException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}