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
    class GetDeckCommand : ProtectedRouteCommand
    {
        private readonly IGameManager GameManager;
        public string Format { get; private set; }

        public GetDeckCommand(IGameManager gameManager, string format)
        {
            GameManager = gameManager;
            Format = format;
        }

        public override Response Execute()
        {
            var response = new Response();
            try
            {
                var cards = GameManager.GetDeck(User);
                response.Payload = Format switch
                {
                    "plain" => PlainFormating(cards),
                    _ => JsonConvert.SerializeObject(cards)
                };
                
                response.StatusCode = StatusCode.Ok;
            }
            catch (Exception)
            {
                response.Payload = "Failed to fetch deck";
                response.StatusCode = StatusCode.Unauthorized;
            }
            return response;
        }

        private string PlainFormating(ICollection<Card> cards)
        {
            string payload = null;
            foreach (var card in cards)
                payload += $"\nID: {card.Id}\nName: {card.Name}\nDamage: {card.Damage}\nElement: {card.Element}\n";
            return payload;
        }
    }
}
