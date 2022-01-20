using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    [Serializable]
    internal class UnconfiguredDeckException : Exception
    {
        public UnconfiguredDeckException()
        {
        }

        public UnconfiguredDeckException(string message) : base(message)
        {
        }

        public UnconfiguredDeckException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
