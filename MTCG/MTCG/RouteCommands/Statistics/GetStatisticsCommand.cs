using MTCG.Core.Response;
using Newtonsoft.Json;
using System;

namespace MTCG.RouteCommands.Statistics
{
    class GetStatisticsCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;

        public GetStatisticsCommand(IGameManager gameManager)
        {
            GameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                response.Payload = JsonConvert.SerializeObject(GameManager.GetStatistics(User));
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
