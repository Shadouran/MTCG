using MTCG.Core.Response;
using Newtonsoft.Json;
using System;

namespace MTCG.RouteCommands.Cards
{
    class GetTradesCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;

        public GetTradesCommand(IGameManager gameManager)
        {
            GameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                response.Payload = JsonConvert.SerializeObject(GameManager.GetTrades());
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception e)
            {
                response.Payload = e.Message;
                response.StatusCode = StatusCode.BadRequest;
            }
            return response;
        }
    }
}
