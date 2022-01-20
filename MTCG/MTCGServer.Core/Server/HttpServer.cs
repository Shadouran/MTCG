using MTCG.Core.Client;
using MTCG.Core.Listener;
using MTCG.Core.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Core.Server
{
    public class HttpServer : IServer
    {
        private readonly IListener listener;
        private readonly IRouter router;
        private bool isListening;
        private readonly List<Task> Tasks = new();

        public HttpServer(IPAddress address, int port, IRouter router)
        {
            listener = new Listener.HttpListener(address, port);
            this.router = router;
        }

        public void Start()
        {
            listener.Start();
            isListening = true;

            while (isListening)
            {
                try
                {
                    var client = listener.AcceptClient();
                    Tasks.Add(Task.Run(() => HandleClient(client)));
                }
                catch (Exception)
                {
                }
            }
            foreach (var task in Tasks)
                task.Wait();
        }

        public void Stop()
        {
            listener.Stop();
            isListening = false;
        }

        private void HandleClient(IClient client)
        {
            var request = client.ReceiveRequest();

            Response.Response response;
            try
            {
                var command = router.Resolve(request);
                if (command != null)
                {
                    response = command.Execute();
                }
                else
                {
                    response = new Response.Response()
                    {
                        StatusCode = Response.StatusCode.BadRequest
                    };
                }
            }
            catch (RouteNotAuthorizedException)
            {
                response = new Response.Response()
                {
                    StatusCode = Response.StatusCode.Unauthorized
                };
            }

            client.SendResponse(response);
        }
    }
}
