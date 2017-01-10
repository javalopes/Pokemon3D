using System;
using System.Threading;
using System.Net;
using Pokemon3D.Common.Diagnostics;

namespace Pokemon3D.Server
{
    /// <summary>
    /// Provides a listener to http streams.
    /// </summary>
    public class HttpServer
    {
        private const string ServerApi = "p3dapi";
        private readonly HttpListener _listener = new HttpListener();
        private const string FormatUrl = "http://{0}:{1}/{2}/";

        private readonly FileRequestHandler _requestHandler;

        public HttpServer(string gameModeFolder, string host, string port)
        {
            CheckSupported();

            string url = string.Format(FormatUrl, host, port, ServerApi);
            _listener.Prefixes.Add(url);

            // setup request handler:
            _requestHandler = new FileRequestHandler(gameModeFolder);

            // start http listener:
            _listener.Start();
        }
        
        private static void CheckSupported()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Windows Server 2003 or later.");
        }

        /// <summary>
        /// Starts the <see cref="HttpServer"/>.
        /// </summary>
        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {
                GameLogger.Instance.Log(MessageType.Message, "Webserver running...");

                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var context = c as HttpListenerContext;
                            try
                            {
                                var buf = _requestHandler.HandleRequest(GetRequestPath(context.Request));
                                context.Response.ContentLength64 = buf.Length;
                                context.Response.OutputStream.Write(buf, 0, buf.Length);
                            }
                            catch
                            {
                                // ignored
                            } // suppress exceptions
                            finally
                            {
                                // close stream
                                context?.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch
                {
                    // ignored
                } // suppress exceptions
            });
        }

        private string GetRequestPath(HttpListenerRequest request)
        {
            string requestUrl = request.Url.PathAndQuery;
            requestUrl = requestUrl.TrimStart('/');
            requestUrl = requestUrl.Remove(0, ServerApi.Length);
            requestUrl = requestUrl.TrimStart('/');
            return requestUrl;
        }

        /// <summary>
        /// Stops the <see cref="HttpServer"/>.
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}
