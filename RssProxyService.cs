using System;
using System.ServiceProcess;

namespace RssProxy
{
    public partial class RssProxyService : ServiceBase
    {
        private WebServer _webServer;

        public RssProxyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var options = new WebServerOptions
            {
                UrlPrefix = Environment.GetCommandLineArgs()[1],
            };
            _webServer = new WebServer();
            _webServer.Start(options);
        }

        protected override void OnStop()
        {
            if (_webServer != null)
            {
                _webServer.Dispose();
                _webServer = null;
            }
        }
    }
}
