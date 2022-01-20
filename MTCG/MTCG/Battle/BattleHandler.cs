using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle
{
    public class BattleHandler : IBattleHandler
    {
        private ConcurrentQueue<User> QueuedUsers = null;
        private ConcurrentDictionary<Tuple<CardType, Element>, Dictionary<Tuple<CardType, Element>, double>> WeaknessChart = null;
        private const int MaximumTurns = 101;

        public BattleHandler()
        {
            if (WeaknessChart == null)
                SetupWeaknessChart();
            if (QueuedUsers == null)
                QueuedUsers = new();
        }

        public int GetUserNumberInQueue()
        {
            return QueuedUsers.Count;
        }

        public void QueueUser(User user)
        {
            if(!QueuedUsers.Contains(user))
                QueuedUsers.Enqueue(user);
        }

        public IBattleLogger StartBattle(IDeckRepository deckRepository)
        {
            if(QueuedUsers.TryDequeue(out User userA) && QueuedUsers.TryDequeue(out User userB))
            {
                var deckUserA = deckRepository.GetCardsByAuthToken(userA.Token);
                var deckUserB = deckRepository.GetCardsByAuthToken(userB.Token);
                if (deckUserA.Count == 0 || deckUserB.Count == 0)
                    throw new UnconfiguredDeckException();
                var battleLogger = new BattleLogger(userA.Username, userB.Username);
                return Battle(deckUserA, deckUserB, battleLogger);
            }
            return new BattleLogger("", "");
        }

        private IBattleLogger Battle(ICollection<Card> deckUserA, ICollection<Card> deckUserB, IBattleLogger battleLogger)
        {
            var round = 1;
            while(round <= 100 && deckUserA.Count != 0 && deckUserB.Count != 0)
            {
                var cardA = GetRandomCard(deckUserA);
                var cardB = GetRandomCard(deckUserB);
                double damageA = DamageCalculation(cardA, cardB);
                double damageB = DamageCalculation(cardB, cardA);
                switch(damageA - damageB)
                {
                    // UserA won round
                    case > 0:
                        deckUserA.Add(cardB);
                        deckUserB.Remove(cardB);
                        break;
                    // UserB won round
                    case < 0:
                        deckUserA.Remove(cardA);
                        deckUserB.Add(cardA);
                        break;
                    // Draw
                    default:
                        break;
                }
                battleLogger.LogBattle(round, cardA, damageA, cardB, damageB);
                round++;
            }
            if (deckUserA.Count == 0)
                battleLogger.Conclusion(true, false);
            else if (deckUserB.Count == 0)
                battleLogger.Conclusion(false, true);
            else if (round == MaximumTurns)
                battleLogger.Conclusion(false, false);
            return battleLogger;
        }

        private static Card GetRandomCard(ICollection<Card> deck)
        {
            return deck.ElementAt(new Random().Next(0, deck.Count()));
        }

        public double DamageCalculation(Card cardA, Card cardB)
        {
            if(WeaknessChart.TryGetValue(new Tuple<CardType, Element>(cardA.CardType, cardA.Element), out var multiplierDictA))
                if(multiplierDictA.TryGetValue(new Tuple<CardType, Element>(cardB.CardType, cardB.Element), out var multiplierA))
                    return cardA.Damage * multiplierA;
            return cardA.Damage;
        }

        private void SetupWeaknessChart()
        {
            WeaknessChart = new();

            var key = new Tuple<CardType, Element>(CardType.Goblin, Element.Regular);
            var value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Regular), 0 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Fire), 0 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Water), 0 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 2},
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Goblin, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0.5}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Goblin, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);;

            key = new Tuple<CardType, Element>(CardType.Dragon, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Elf, Element.Fire), 0 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Dragon, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0.5}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Dragon, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Ork, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Regular), 0 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Fire), 0 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Water), 0 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Ork, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0.5}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Ork, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Knight, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0 }
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Knight, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Knight, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Kraken, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0 }
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Kraken, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0.5}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Kraken, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Elf, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 2 }
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Elf, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0.5}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Elf, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Spell, Element.Regular);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Goblin, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Goblin, Element.Water), 2 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Water), 2 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Water), 2 },
                { new Tuple<CardType, Element>(CardType.Ork, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Ork, Element.Water), 2 },
                { new Tuple<CardType, Element>(CardType.Knight, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Knight, Element.Water), 2 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Regular), 0 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Fire), 0 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Water), 0 },
                { new Tuple<CardType, Element>(CardType.Elf, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Elf, Element.Water), 2 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 0.5 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 2 }
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Spell, Element.Fire);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Goblin, Element.Regular), 2 },
                { new Tuple<CardType, Element>(CardType.Goblin, Element.Water), 0.5 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Regular), 2 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Water), 0.5 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Regular), 2 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Water), 0.5 },
                { new Tuple<CardType, Element>(CardType.Ork, Element.Regular), 2 },
                { new Tuple<CardType, Element>(CardType.Ork, Element.Water), 0.5 },
                { new Tuple<CardType, Element>(CardType.Knight, Element.Regular), 2 },
                { new Tuple<CardType, Element>(CardType.Knight, Element.Water), 0.5 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Regular), 0 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Fire), 0 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Water), 0 },
                { new Tuple<CardType, Element>(CardType.Elf, Element.Regular), 2 },
                { new Tuple<CardType, Element>(CardType.Elf, Element.Water), 0.5 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 2},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Water), 0.5}
            };
            WeaknessChart.TryAdd(key, value);

            key = new Tuple<CardType, Element>(CardType.Spell, Element.Water);
            value = new Dictionary<Tuple<CardType, Element>, double> {
                { new Tuple<CardType, Element>(CardType.Goblin, Element.Regular), 0.5 },
                { new Tuple<CardType, Element>(CardType.Goblin, Element.Fire), 2 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Regular), 0.5 },
                { new Tuple<CardType, Element>(CardType.Dragon, Element.Fire), 2 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Regular), 0.5 },
                { new Tuple<CardType, Element>(CardType.Wizzard, Element.Fire), 2 },
                { new Tuple<CardType, Element>(CardType.Ork, Element.Regular), 0.5 },
                { new Tuple<CardType, Element>(CardType.Ork, Element.Fire), 2 },
                { new Tuple<CardType, Element>(CardType.Knight, Element.Regular), 0.5 },
                { new Tuple<CardType, Element>(CardType.Knight, Element.Fire), 2 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Regular), 0 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Fire), 0 },
                { new Tuple<CardType, Element>(CardType.Kraken, Element.Water), 0 },
                { new Tuple<CardType, Element>(CardType.Elf, Element.Regular), 0.5 },
                { new Tuple<CardType, Element>(CardType.Elf, Element.Fire), 2 },
                { new Tuple<CardType, Element>(CardType.Spell, Element.Regular), 0.5},
                { new Tuple<CardType, Element>(CardType.Spell, Element.Fire), 2}
            };
            WeaknessChart.TryAdd(key, value);
        }
    }
}
