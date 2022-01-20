using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Card
    {
        public string Id { get; set; }
        private string name;
        public string Name
        {
            get => name;
            set
            {
                Element = Element.Regular;
                foreach (Element element in Enum.GetValues(typeof(Element)))
                    if (value.Contains(element.ToString()))
                    {
                        Element = element;
                        break;
                    }
                CardType = CardType.Spell;
                foreach (CardType cardType in Enum.GetValues(typeof(CardType)))
                    if(value.Contains(cardType.ToString()))
                        CardType = cardType;
                name = value;
            }
        }
        public double Damage { get; set; }
        public Element Element { get; set; }
        public CardType CardType { get; set; }
    }
}
