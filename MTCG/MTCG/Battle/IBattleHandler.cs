using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Battle
{
    interface IBattleHandler
    {
        void QueueUser(User user);
        int GetUserNumberInQueue();
        IBattleLogger StartBattle(IDeckRepository deckRepository);
    }
}
