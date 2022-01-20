using MTCG.Core.Response;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.RouteCommands.Cards
{
    class AddCardPackageCommando : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public ICollection<Card> Cards { get; private set; }

        public AddCardPackageCommando(IGameManager gameManager, ICollection<Card> cards)
        {
            GameManager = gameManager;
            Cards = cards;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                GameManager.AddCardPackage(Cards, User);
                response.Payload = "Card package added successfully";
                response.StatusCode = StatusCode.Created;
            }
            catch (Exception)
            {
                response.Payload = "Failed to add card package";
                response.StatusCode = StatusCode.Unauthorized;
            }
            return response;
        }
    }
}
