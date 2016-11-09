using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

using Deimos.Logging;

namespace Deimos.Http.Handlers
{
    public class DirectoryHandler : IHttpHandler
    {
        private static ILogger Logger = Log.CreateLogger("DirectoryHandler");

        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>
        {
            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/x-javascript" },
            { ".json", "application/json" },
            { ".jsx", "text/jsx" },
            { ".txt", "text/plain" }
        }; 

        private readonly DirectoryInfo _baseDir;
        private readonly string _prefix;

        public DirectoryHandler(DirectoryInfo baseDir, string prefix)
        {
            if (!prefix.EndsWith("/"))
            {
                prefix = prefix + "/";
            }

            _baseDir = baseDir;
            _prefix = prefix;
        }

        public string DefaultFile
        {
            get; set;
        }

        #region Implementation of IHttpHandler

        public bool ProcessRequest(HttpListenerContext requestContext)
        {
            // Only GET requests inside the prefix are supported
            if (requestContext.Request.HttpMethod != HttpMethod.Get.ToString() || !requestContext.Request.Url.AbsolutePath.StartsWith(_prefix))
            {
                return false;
            }

            string relPath = requestContext.Request.Url.AbsolutePath.Substring(_prefix.Length);

            if (Path.DirectorySeparatorChar != '/')
            {
                relPath = relPath.Replace('/', Path.DirectorySeparatorChar);
            }

            if (relPath == "" && DefaultFile != null)
            {
                relPath = DefaultFile;
            }

            string filename = Path.Combine(_baseDir.FullName, relPath);

            if (File.Exists(filename))
            {
                Logger.Debug("Serving {0}...", filename);

                try
                {
                    Stream input = new FileStream(filename, FileMode.Open);

                    //Adding permanent http response headers
                    string mime;
                    requestContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    requestContext.Response.ContentType = MimeTypes.TryGetValue(Path.GetExtension(filename), out mime) ? mime : "application/octet-stream";
                    requestContext.Response.ContentLength64 = input.Length;
                    requestContext.Response.AddHeader("Date", DateTime.Now.ToString("r"));
                    requestContext.Response.AddHeader("Last-Modified", File.GetLastWriteTime(filename).ToString("r"));

                    byte[] buffer = new byte[1024 * 16];
                    int nbytes;
                    while ((nbytes = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        requestContext.Response.OutputStream.Write(buffer, 0, nbytes);
                        Logger.Debug("{0} bytes copied", nbytes);
                    }
                    input.Close();
                    requestContext.Response.OutputStream.Flush();
                }
                catch (Exception ex)
                {
                    Logger.Debug("Exception: {0}", ex.Message);
                    Logger.Debug("{0}", ex.StackTrace);
                    requestContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                }

                requestContext.Response.OutputStream.Close();

                Logger.Debug("Done.");

                return true;
            }

            return false;
        }

        #endregion
    }
}
