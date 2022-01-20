using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    interface ICardRepository
    {
        void InsertCard(Card card);
        void InsertCards(ICollection<Card> cards);
        void RemoveCard(Card card);
        void UpdateCard(Card card);
        Card SelectCard(string offerId);
    }
}
