using MTCG.Core.Response;
using System;

namespace MTCG.RouteCommands.Cards
{
    class TradeCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public string TradeId { get; private set; }
        public string OfferId { get; private set; }

        public TradeCommand(IGameManager gameManager, string tradeId, string offerId)
        {
            GameManager = gameManager;
            TradeId = tradeId;
            OfferId = offerId;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.Trade(User.Username, TradeId, OfferId);
                response.Payload = "Trade successful";
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
