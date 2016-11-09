using System.Net;
using System.Text;

using Newtonsoft.Json;

namespace Deimos.Http
{
    public static class HttpListenerContextExtensions
    {
        /// <summary>
        /// Sends the data object as UTF8 encoded JSON response.
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="data"></param>
        /// <returns>Always <c>true</c>.</returns>
        public static bool Json(this HttpListenerContext ctx, object data)
        {
            byte[] jsonData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

            ctx.Response.ContentEncoding = Encoding.UTF8;
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = (int)HttpStatusCode.OK;
            ctx.Response.OutputStream.Write(jsonData, 0, jsonData.Length);
            ctx.Response.OutputStream.Close();

            return true;
        }
    }
}
