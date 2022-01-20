using MTCG.Core.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.RouteCommands.Cards
{
    class SetDeckCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public IEnumerable<string> CardIds { get; private set; }

        public SetDeckCommand(IGameManager gameManager, IEnumerable<string> cardIds)
        {
            GameManager = gameManager;
            CardIds = cardIds;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.SetDeck(User, CardIds);
                response.Payload = "Updated deck successfully";
                response.StatusCode = StatusCode.Created;
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
