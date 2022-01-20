using MTCG.Core.Response;
using System;

namespace MTCG.RouteCommands.Cards
{
    class DeleteTradeCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public string TradeId { get; private set; }

        public DeleteTradeCommand(IGameManager gameManager, string tradeId)
        {
            GameManager = gameManager;
            TradeId = tradeId;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.DeleteTrade(User.Username, TradeId);
                response.Payload = "Deleted trade successfully";
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
