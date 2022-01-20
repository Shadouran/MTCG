using MTCG.Core.Response;
using MTCG.Core.Routing;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.RouteCommands.Users
{
    class LoginCommand : IRouteCommand
    {
        private readonly IGameManager GameManager;

        public Credentials Credentials { get; private set; }

        public LoginCommand(IGameManager messageManager, Credentials credentials)
        {
            Credentials = credentials;
            GameManager = messageManager;
        }

        public Response Execute()
        {
            User user;
            try
            {
                user = GameManager.LoginUser(Credentials);
            }
            catch (UserNotFoundException)
            {
                user = null;
            }

            var response = new Response();
            if (user == null)
            {
                response.StatusCode = StatusCode.Unauthorized;
                response.Payload = "Username or password is wrong";
            }
            else
            {
                response.StatusCode = StatusCode.Ok;
                response.Payload = user.Token;
            }

            return response;
        }
    }
}
