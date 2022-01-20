using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    [Serializable]
    internal class InvalidCardCountException : Exception
    {
        public InvalidCardCountException()
        {
        }

        public InvalidCardCountException(string message) : base(message)
        {
        }

        public InvalidCardCountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
