using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    public interface IDeckRepository
    {
        ICollection<Card> GetCardsByAuthToken(string authToken);
        void InsertCardsByAuthToken(string authToken, IEnumerable<string> cardIds);
        void InsertCardByAuthToken(string authToken, string cardId);
        void ResetDeck(string authToken);
        Card SelectCard(string cardId);
    }
}
