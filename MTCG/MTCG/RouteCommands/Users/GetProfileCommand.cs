using MTCG.Core.Response;
using MTCG.Models;
using Newtonsoft.Json;
using System;

namespace MTCG.RouteCommands.Users
{
    class GetProfileCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public string Username { get; private set; }

        public GetProfileCommand(IGameManager gameManager, string username)
        {
            GameManager = gameManager;
            Username = username;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                response.Payload = JsonConvert.SerializeObject(GameManager.GetUserProfile(User, Username));
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception e)
            {
                response.Payload = e.Message;
                response.StatusCode = StatusCode.Unauthorized;
            }
            return response;
        }
    }
}
