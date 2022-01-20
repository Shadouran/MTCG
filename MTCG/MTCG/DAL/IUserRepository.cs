using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    public interface IUserRepository
    {
        User GetUserByCredentials(string username, string password);
        User GetUserByAuthToken(string authToken);
        User GetUserProfileByAuthToken(string authToken);
        void SetUserProfileByAuthToken(string authToken, User userProfile);
        bool InsertUser(User user);
        int UpdateCoinsByAuthToken(string authToken, int coins);
    }
}
