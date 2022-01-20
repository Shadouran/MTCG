using MTCG.Core.Response;
using MTCG.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.RouteCommands.Cards
{
    class GetStackCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;

        public GetStackCommand(IGameManager gameManager)
        {
            GameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                var cards = GameManager.GetStack(User);
                response.Payload = JsonConvert.SerializeObject(cards);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                response.Payload = "Failed to fetch stack";
                response.StatusCode = StatusCode.Forbidden;
            }
            return response;
        }
    }
}
