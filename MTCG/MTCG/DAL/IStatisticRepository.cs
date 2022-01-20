using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    interface IStatisticRepository
    {
        Dictionary<string, Statistics> SelectStatistics();
        Statistics SelectStatisticsByAuthToken(string authToken);
        Statistics SelectStatisticsByUsername(string username);
        void InsertStatisticsByUsername(string username);
        void UpdateELOByUsername(string username, int elo);
        void UpdateWinsByUsername(string username, int wins);
        void UpdateLossesByUsername(string username, int losses);
        void UpdateGamesPlayedByUsername(string username, int gamesPlayed);
    }
}
