using MTCG.Core.Request;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    interface IGameManager
    {
        User LoginUser(Credentials credentials);
        void RegisterUser(Credentials credentials);
        User GetUserProfile(User user, string username);
        void UpdateUserProfile(User user, User userProfile, string username);

        void AddCardPackage(ICollection<Card> cards, User user);
        CardPackage PurchaseFirstCardPackage(User user);

        ICollection<Card> GetStack(User user);
        ICollection<Card> GetDeck(User user);
        void SetDeck(User user, IEnumerable<string> cardIds);

        ICollection<Trade> GetTrades();
        void PostTrade(string username, Trade trade);
        void DeleteTrade(string username, string tradeId);
        void Trade(string username, string tradeId, string offerId);

        Statistics GetStatistics(User user);
        Dictionary<string, Statistics> GetScoreboard();

        string RegisterForBattle(User user);
    }
}
