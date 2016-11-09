using System.Net;

using Deimos.Http;
using Deimos.Logging;
using System;

namespace RequestLogger
{
    class Program
    {
        private class RequestInfo
        {
            public string Path { get; set; }

            public string Method { get; set; }

            public RequestInfo(HttpListenerRequest req)
            {
                Path = req.Url.AbsolutePath;
                Method = req.HttpMethod;
            }
        }

        private class Handler : IHttpHandler
        {
            private static ILogger Logger = Log.CreateLogger("Handler");

            public bool ProcessRequest(HttpListenerContext requestContext)
            {
                Logger.Info("Path: {0}", requestContext.Request.Url.AbsolutePath);
                Logger.Info("Method: {0}", requestContext.Request.HttpMethod);

                return requestContext.Json(new RequestInfo(requestContext.Request));
            }
        }

        static void Main(string[] args)
        {
            Log.ToConsole();

            if (args.Length != 1)
            {
                Console.WriteLine("Usage: RequestLogger <port number>", args[0]);
                return;
            }

            int portNumber;
            if (!int.TryParse(args[0], out portNumber))
            {
                Console.WriteLine("Illegal port number.", args[0]);
                return;
            }

            HttpServer server = new HttpServer(portNumber);
            server.AddHandler(new Handler());
            server.Run();
        }
    }
}
