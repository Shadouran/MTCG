using MTCG.Core.Request;
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
    class PurchaseRandomPackageCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;

        public PurchaseRandomPackageCommand(IGameManager gameManager)
        {
            GameManager = gameManager;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                CardPackage cardPackage = GameManager.PurchaseFirstCardPackage(User);
                response.Payload = JsonConvert.SerializeObject(cardPackage.Cards);
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                response.Payload = "Failed to purchase package";
                response.StatusCode = StatusCode.Ok;
            }
            return response;
        }
    }
}
