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
    class RegisterCommand : IRouteCommand
    {
        private readonly IGameManager GameManager;
        public Credentials Credentials { get; private set; }

        public RegisterCommand(IGameManager gameManager, Credentials credentials)
        {
            Credentials = credentials;
            GameManager = gameManager;
        }

        public Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.RegisterUser(Credentials);
                response.Payload = "User successfully created";
                response.StatusCode = StatusCode.Created;
            }
            catch (DuplicateUserException)
            {
                response.Payload = "Failed to create user";
                response.StatusCode = StatusCode.Conflict;
            }

            return response;
        }
    }
}
