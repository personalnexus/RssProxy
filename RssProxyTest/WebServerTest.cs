using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RssProxy;

namespace RssProxyTest
{
    [TestClass]
    public class WebServerTest
    {
        [TestMethod]
        public void ConvertRequestUrl()
        {
            var input = new Uri("http://localhost:8080/example.com/go/rss?lang=en-us&q=something");
            Assert.AreEqual("https://example.com/go/rss?lang=en-us&q=something", WebServer.ConvertRequestUrl(input));
        }
    }
}
