using MTCG.Core.Response;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MTCG.RouteCommands
{
    class BattleCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;

        public BattleCommand(IGameManager gameManager)
        {
            GameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                response.Payload = GameManager.RegisterForBattle(User);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                response.Payload = "Failed to find another player";
                response.StatusCode = StatusCode.NoContent;
            }
            return response;
        }
    }
}
