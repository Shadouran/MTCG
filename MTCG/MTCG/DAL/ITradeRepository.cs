using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    interface ITradeRepository
    {
        ICollection<Trade> SelectTrades();
        void InsertTrade(Trade trade);
        Trade SelectTrade(string tradeId);
        void DeleteTrade(string username, string tradeId);
        ICollection<Trade> SelectTradesByUsername(string username);
    }
}
