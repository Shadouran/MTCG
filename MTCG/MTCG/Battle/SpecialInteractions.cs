using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle
{
    class SpecialInteractions
    {
        private static Dictionary<Tuple<CardType, Element>, Dictionary<Tuple<CardType, Element>, string>> Chart = null;

        public static string Interaction(Card cardA, Card cardB)
        {
            if (Chart == null)
                InitChart();
            var interactions = "";
            if (Chart.TryGetValue(new Tuple<CardType, Element>(cardA.CardType, cardA.Element), out var dict))
                if (dict.TryGetValue(new Tuple<CardType, Element>(cardB.CardType, cardB.Element), out var lore))
                    interactions += lore;
            if (Chart.TryGetValue(new Tuple<CardType, Element>(cardB.CardType, cardB.Element), out dict))
                if (dict.TryGetValue(new Tuple<CardType, Element>(cardA.CardType, cardA.Element), out var lore))
                    interactions += lore;
            return interactions;
        }

        private static void InitChart()
        {
            Chart = new();

            var key = new Tuple<CardType, Element>(CardType.Goblin, Element.Regular);
            var value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Regular), "The goblin is too scared to fight the dragon.\n" },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Fire), "The goblin is too scared to fight the fiery dragon.\n" },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Water), "The goblin is too scared to fight the dragon.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Dragon, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Elf, Element.Fire), "Knowing the moves of the dragon, the elf evades its attacks easily.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Ork, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Regular), "The wizzard controls the ork's movements, making it unable to attack.\n" },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Fire), "The wizzard controls the ork's movements, making it unable to attack.\n" },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Water), "The wizzard controls the ork's movements, making it unable to attack.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Spell, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Regular), "The kraken nullifies the spell thrown at it.\n" },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Fire), "The kraken nullifies the spell thrown at it.\n" },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Water), "The kraken nullifies the spell thrown at it.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Spell, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Regular), "The kraken nullifies the spell thrown at it.\n" },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Fire), "The kraken nullifies the spell thrown at it.\n" },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Water), "The kraken nullifies the spell thrown at it.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Spell, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Regular), "The kraken nullifies the spell thrown at it.\n" },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Fire), "The kraken nullifies the spell thrown at it.\n" },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Water), "The kraken nullifies the spell thrown at it.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Knight, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), "The knight's armor is too heavy to swim in, disabling his movements.\n" }
            };
            Chart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Knight, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, string> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), "The knight's armor is too heavy to swim in, disabling his movements.\n" }
            };
            Chart.TryAdd(key, value);
        }
    }
}
