using MTCG.Core.Response;
using Newtonsoft.Json;
using System;

namespace MTCG.RouteCommands.Statistics
{
    class GetScoreboardCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;

        public GetScoreboardCommand(IGameManager gameManager)
        {
            GameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                response.Payload = JsonConvert.SerializeObject(GameManager.GetScoreboard());
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                response.Payload = "Failed to fetch scoreboard";
                response.StatusCode = StatusCode.InternalServerError;
            }
            return response;
        }
    }
}
