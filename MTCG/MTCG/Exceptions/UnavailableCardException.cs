using System;
using System.Runtime.Serialization;

namespace MTCG
{
    [Serializable]
    internal class UnavailableCardException : Exception
    {
        public UnavailableCardException()
        {
        }

        public UnavailableCardException(string message) : base(message)
        {
        }

        public UnavailableCardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}