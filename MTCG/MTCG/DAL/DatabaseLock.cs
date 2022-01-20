using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    class DatabaseLock
    {
        public static readonly Semaphore Semaphore = new(1, 1);
        public static readonly Semaphore DeckSemaphore = new(1, 1);
    }
}
