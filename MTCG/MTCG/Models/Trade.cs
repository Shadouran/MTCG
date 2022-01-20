using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    class Trade
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string CardId { get; set; }
        public CardType CardType { get; set; }
        public Element Element { get; set; }
        public double MinimumDamage { get; set; }
    }
}
