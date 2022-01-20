using System;
using System.Runtime.Serialization;

namespace MTCG
{
    [Serializable]
    internal class NoTradesFoundException : Exception
    {
        public NoTradesFoundException()
        {
        }

        public NoTradesFoundException(string message) : base(message)
        {
        }

        public NoTradesFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoTradesFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}