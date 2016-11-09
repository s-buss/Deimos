using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

using Deimos.Logging;

namespace Deimos.Http
{
    public class HttpServer
    {
        private static ILogger Logger = Log.CreateLogger("HttpServer");

        private readonly int _portNumber;
        private readonly List<IHttpHandler> _handlers = new List<IHttpHandler>();

        private Thread _thread;
        private HttpListener _listener;

        public HttpServer(int portNumber)
        {
            _portNumber = portNumber;
        }

        public void AddHandler(IHttpHandler handler)
        {
            _handlers.Add(handler);
        }

        public void Start()
        {
            _thread = new Thread(Run);
            _thread.Start();
        }

        public void Stop()
        {
            _listener.Close();
            //_thread.Abort();
        }

        public void Run()
        {
            Logger.Info("Listening on port {0}", _portNumber);

            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:" + _portNumber + "/");
            _listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    ProcessRequest(context);
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (Exception)
                {
                }
            }
        }

        private void ProcessRequest(HttpListenerContext requestContext)
        {
            Logger.Debug(requestContext.Request.HttpMethod + " " + requestContext.Request.Url.AbsolutePath);

            try
            {
                bool handled = false;
                foreach (IHttpHandler handler in _handlers)
                {
                    if (handler.ProcessRequest(requestContext))
                    {
                        handled = true;
                        break;
                    }
                }

                if (!handled)
                {
                    Logger.Debug("No suitable handler found for the path.");

                    requestContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    requestContext.Response.ContentType = "text/plain";
                    requestContext.Response.ContentEncoding = Encoding.UTF8;
                    StreamWriter writer = new StreamWriter(requestContext.Response.OutputStream, Encoding.UTF8);
                    writer.WriteLine("404 - The requested resource could not be found.");
                    writer.Close();
                    requestContext.Response.OutputStream.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception in HTTP handler: {0}", ex.Message);

                requestContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                requestContext.Response.ContentType = "text/plain";
                requestContext.Response.ContentEncoding = Encoding.UTF8;
                StreamWriter writer = new StreamWriter(requestContext.Response.OutputStream, Encoding.UTF8);
                writer.WriteLine("500 - An internal server error occurred while processing the request.");
                writer.Close();
                requestContext.Response.OutputStream.Close();
            }
        }
    }
}
