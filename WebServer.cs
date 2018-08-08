using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace RssProxy
{
    public class WebServer : IDisposable
    {
        public void Start(WebServerOptions options)
        {
            LogEvent(EventLogEntryType.Information, $"Starting web server at {options.UrlPrefix}");

            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(options.UrlPrefix);
            _httpListener.Start();

            Task.Run(new Action(ProcessRequests));
        }

        private HttpListener _httpListener;

        private void ProcessRequests()
        {
            // copy to local, because field is nulled when listener is stopped
            HttpListener httpListener = _httpListener;
            if (httpListener != null)
            {
                while (httpListener.IsListening)
                {
                    try
                    {
                        HttpListenerContext context = httpListener.GetContext();
                        ProcessRequest(context);
                    }
                    catch (Exception exception)
                    {
                        // Ignore exceptions if _httpListener has been stopped
                        if (httpListener.IsListening)
                        {
                           LogEvent(EventLogEntryType.Error, $"An unexpected error occurred while processing requests: {exception.Message}\r\n{exception.StackTrace}");
                        }
                        continue;
                    }
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            string address = "https:/" + context.Request.Url.AbsolutePath;
            LogEvent(EventLogEntryType.Information, "Downloading {address}...");
            try
            {
                using (var webClient = new WebClient())
                {
                    string page = webClient.DownloadString(address);
                    using (var writer = new StreamWriter(context.Response.OutputStream, webClient.Encoding))
                    {
                        writer.Write(page);
                    }
                }
            }
            catch (WebException exception)
            {
                HttpStatusCode? status = (exception.Response as HttpWebResponse)?.StatusCode;
                context.Response.StatusCode = status.HasValue ? (int)status.Value : 500;
                throw new Exception($"Error {context.Response.StatusCode} occurred while processing a request for {context.Request.RawUrl}", exception);
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                throw;
            }
            finally
            {
                context.Response.Close();
            }
        }

        private void LogEvent(EventLogEntryType entryType, string message)
        {
            //TODO: add event logging
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_httpListener != null)
                {
                    _httpListener.Stop();
                    _httpListener.Close();
                    _httpListener = null;
                }
            }
        }
    }
}