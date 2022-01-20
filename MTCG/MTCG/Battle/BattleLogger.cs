using MTCG.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle
{
    class BattleLogger : IBattleLogger
    {
        private string UsernamePlayerA { get; set; }
        private string UsernamePlayerB { get; set; }
        public string UsernameWinner { get; private set; }
        public string UsernameLoser { get; private set; }
        public string Log { get; private set; }
        private int WinsPlayerA { get; set; }
        private int WinsPlayerB { get; set; }
        private int Draws { get; set; }

        public BattleLogger(string usernamePlayerA, string usernamePlayerB)
        {
            UsernamePlayerA = usernamePlayerA;
            UsernamePlayerB = usernamePlayerB;
        }

        public void LogBattle(int round, Card cardA, double damageA, Card cardB, double damageB)
        {
            var log = $"Round {round}\n" + $"{UsernamePlayerA} plays {cardA.Name}\n" + $"{UsernamePlayerB} plays {cardB.Name}\n" + SpecialInteractions.Interaction(cardA, cardB) + $"{cardA.Name} deals {damageA} damage\n" + $"{cardB.Name} deals {damageB} damage\n";
            switch (damageA - damageB)
            {
                // UserA won round
                case > 0:
                    log += $"{UsernamePlayerA} wins this round!\n";
                    WinsPlayerA++;
                    break;
                // UserB won round
                case < 0:
                    log += $"{UsernamePlayerB} wins this round!\n";
                    WinsPlayerB++;
                    break;
                // Draw
                default:
                    log += "Round ends in a draw!\n";
                    Draws++;
                    break;
            }
            Log += log + '\n';
        }

        public void Conclusion(bool playerA, bool playerB)
        {
            if (playerA)
            {
                Log += $"{UsernamePlayerA} wins!\n";
                UsernameWinner = UsernamePlayerA;
                UsernameLoser = UsernamePlayerB;
            }
            else if (playerB)
            {
                Log += $"{UsernamePlayerB} wins!\n";
                UsernameWinner = UsernamePlayerB;
                UsernameLoser = UsernamePlayerA;
            }
            else if (!playerA && !playerB)
                Log += "Draw!\n";
            Log += $"Wins {UsernamePlayerA}: {WinsPlayerA}\n" + $"Wins {UsernamePlayerB}: {WinsPlayerB}\n" + $"Draws: {Draws}\n";
        }

        public string GetLog()
        {
            return Log;
        }

        public string GetWinner()
        {
            return UsernameWinner;
        }

        public string GetLoser()
        {
            return UsernameLoser;
        }

        public string GetPlayerA()
        {
            return UsernamePlayerA;
        }

        public string GetPlayerB()
        {
            return UsernamePlayerB;
        }
    }
}
