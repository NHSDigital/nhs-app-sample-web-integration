using System;
using System.Runtime.Serialization;

namespace Nnhsuk.base_application.TagHelpers
{
    [Serializable]
    public class OptionalValueMissingException : Exception
    {
        public OptionalValueMissingException()
        {
        }

        public OptionalValueMissingException(string message) : base(message)
        {
        }

        public OptionalValueMissingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected OptionalValueMissingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}