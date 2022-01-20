using MTCG.Core.Response;
using MTCG.Models;
using System;

namespace MTCG.RouteCommands.Cards
{
    class PostTradeCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public Trade Trade { get; private set; }

        public PostTradeCommand(IGameManager gameManager, Trade trade)
        {
            GameManager = gameManager;
            Trade = trade;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.PostTrade(User.Username, Trade);
                response.Payload = "Trade posted succesfully";
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
