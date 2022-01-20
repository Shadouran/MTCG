using MTCG.Models;

namespace MTCG.Battle
{
    public interface IBattleLogger
    {
        string GetLog();
        void Conclusion(bool playerA, bool playerB);
        void LogBattle(int round, Card cardA, double damageA, Card cardB, double damageB);
        string GetWinner();
        string GetLoser();
        string GetPlayerA();
        string GetPlayerB();
    }
}