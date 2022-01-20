using System;
using System.Runtime.Serialization;

namespace MTCG
{
    [Serializable]
    internal class InvalidTradeException : Exception
    {
        public InvalidTradeException()
        {
        }

        public InvalidTradeException(string message) : base(message)
        {
        }

        public InvalidTradeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidTradeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}