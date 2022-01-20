using System;
using System.Runtime.Serialization;

namespace MTCG
{
    [Serializable]
    internal class InvalidCardException : Exception
    {
        public InvalidCardException()
        {
        }

        public InvalidCardException(string message) : base(message)
        {
        }

        public InvalidCardException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidCardException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}