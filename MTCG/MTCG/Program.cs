using System;
using System.Net;
using Newtonsoft.Json;
using MTCG.Core.Request;
using MTCG.Core.Routing;
using MTCG.Core.Server;
using MTCG.DAL;
using MTCG.Models;
using MTCG.RouteCommands.Users;
using MTCG.RouteCommands.Cards;
using System.Collections.Generic;
using MTCG.RouteCommands.Statistics;
using MTCG.RouteCommands;
using System.Threading.Tasks;
using MTCG.Battle;

namespace MTCG
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Database("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=mtcg");

            var battleHandler = new BattleHandler();
            var gameManager = new GameManager(db.CardPackageRepository,
                                                db.StackRepository,
                                                db.UserRepository,
                                                db.CardRepository,
                                                db.DeckRepository,
                                                db.StatisticRepository,
                                                db.TradeRepository,
                                                battleHandler);

            var identityProvider = new IdentityProvider(db.UserRepository);
            var routeParser = new RouteParser();

            var router = new Router(routeParser, identityProvider);
            RegisterRoutes(router, gameManager);

            var httpServer = new HttpServer(IPAddress.Any, 10001, router);
            Task.Run(() => ConsoleCommands(ref httpServer));
            httpServer.Start();
        }

        private static void RegisterRoutes(Router router, IGameManager gameManager)
        {
            // public routes
            router.AddRoute(HttpMethod.Post, "/sessions", (r, p) => new LoginCommand(gameManager, Deserialize<Credentials>(r.Payload)));
            router.AddRoute(HttpMethod.Post, "/users", (r, p) => new RegisterCommand(gameManager, Deserialize<Credentials>(r.Payload)));

            // protected routes
            router.AddProtectedRoute(HttpMethod.Post, "/packages", (r, p) => new AddCardPackageCommando(gameManager, Deserialize<List<Card>>(r.Payload)));
            router.AddProtectedRoute(HttpMethod.Post, "/transactions/packages", (r, p) => new PurchaseRandomPackageCommand(gameManager));
            router.AddProtectedRoute(HttpMethod.Get, "/cards", (r, p) => new GetStackCommand(gameManager));
            router.AddProtectedRoute(HttpMethod.Get, "/deck", (r, p) => new GetDeckCommand(gameManager, p.GetValueOrDefault("format")));
            router.AddProtectedRoute(HttpMethod.Put, "/deck", (r, p) => new SetDeckCommand(gameManager, Deserialize<List<string>>(r.Payload)));
            router.AddProtectedRoute(HttpMethod.Get, "/users/{id}", (r, p) => new GetProfileCommand(gameManager, p.GetValueOrDefault("_id")));
            router.AddProtectedRoute(HttpMethod.Put, "/users/{id}", (r, p) => new UpdateProfileCommand(gameManager, Deserialize<User>(r.Payload), p.GetValueOrDefault("_id")));
            router.AddProtectedRoute(HttpMethod.Get, "/stats", (r, p) => new GetStatisticsCommand(gameManager));
            router.AddProtectedRoute(HttpMethod.Get, "/score", (r, p) => new GetScoreboardCommand(gameManager));
            router.AddProtectedRoute(HttpMethod.Post, "/battles", (r, p) => new BattleCommand(gameManager));
            router.AddProtectedRoute(HttpMethod.Get, "/trades", (r, p) => new GetTradesCommand(gameManager));
            router.AddProtectedRoute(HttpMethod.Post, "/trades", (r, p) => new PostTradeCommand(gameManager, Deserialize<Trade>(r.Payload)));
            router.AddProtectedRoute(HttpMethod.Delete, "/trades/{id}", (r, p) => new DeleteTradeCommand(gameManager, p["_id"]));
            router.AddProtectedRoute(HttpMethod.Post, "/trades/{id}", (r, p) => new TradeCommand(gameManager, p["_id"], Deserialize<string>(r.Payload)));
        }

        private static T Deserialize<T>(string payload) where T : class
        {
            var deserializedData = JsonConvert.DeserializeObject<T>(payload);
            return deserializedData;
        }

        private static void ConsoleCommands(ref HttpServer server)
        {
            while (true)
            {
                var input = Console.ReadLine();
                if (input == "quit")
                {
                    server.Stop();
                    break;
                }
            }
        }
    }
}
