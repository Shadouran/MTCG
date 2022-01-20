using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Statistics
    {
        public uint ELO { get; set; }
        public uint Wins { get; set; }
        public uint Losses { get; set; }
        public uint GamesPlayed { get; set; }
    }
}
