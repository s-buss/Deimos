using System.Net;

namespace Deimos.Http
{
    public interface IHttpHandler
    {
        bool ProcessRequest(HttpListenerContext requestContext);
    }
}
