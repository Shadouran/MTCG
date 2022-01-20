using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    interface IStackRepository
    {
        void InsertCard(string username, Card card);
        void InsertCards(string username, ICollection<Card> cards);
        void DeleteCard(string username);
        ICollection<Card> GetCardsByAuthToken(string authToken);
        void UpdateOwnerOfCard(string cardId, string username);
    }
}
