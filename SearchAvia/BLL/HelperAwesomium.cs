using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Awesomium.Core;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace SearchAvia
{
    public class HelperAwesomium
    {
        private WebView _browser;
        public bool IsLoading { get; protected set; }

        public HelperAwesomium()
        {
            
        }

        Func<string, bool> _checkLoading;

        public void Load(string url, Func<string, bool> checkLoading)
        {
            this._url = url;
            this._checkLoading = checkLoading;
            //var th = new Thread(LoadThread);
            //th.Start();
            LoadThread();
        }

        private string _url;
        private void LoadThread()
        {
            IsLoading = true;
            Init();
            _browser = CreateBrowser();
            _browser.WebSession.ClearCookies();
            _browser.Source = new Uri(_url);
            _browser.DocumentReady += _browser_DocumentReady;
            while (IsLoading)
            {
                WebCore.Update();
                Thread.Sleep(500);
                var b = _checkLoading.Invoke(GetHtml());
                if (b)
                    IsLoading = false;
                if (_browser.HTML.Length > 0)
                {
                    //var t = _browser.ExecuteJavascriptWithResult("document.querySelector('a');");
                }
            }

            var t = _browser.ExecuteJavascriptWithResult("document.querySelector('.ticket-new__opener');");
        }

        private void _browser_DocumentReady(object sender, DocumentReadyEventArgs e)
        {
            //File.WriteAllText(@"C:\temp\2\3.html", (sender as WebView).HTML);
        }

        public void Stop()
        {
            IsLoading = false;
        }

        public string GetHtml()
        {
            if (_browser == null || _browser.IsLoading)
                return "";
            else
            {
                var html = _browser.ExecuteJavascriptWithResult("document.body.innerHTML");
                return html.ToString();
            }
        }

        private static object _lock = new object();
        private void Init()
        {
            lock (_lock)
            {
                if (!WebCore.IsInitialized)
                {
                    WebConfig.Default.UserAgent = "Chrome";
                    WebCore.Initialize(WebConfig.Default);
                }
            }
        }

        public void Test2()
        {
            JSObject btn = _browser.ExecuteJavascriptWithResult("document.getElementsByTagName('a')[0]");
        }

        private static WebView CreateBrowser()
        {
            var p = WebPreferences.Default;
            
            WebSession session = WebCore.CreateWebSession(p);
            return WebCore.CreateWebView(1100, 600, session);
        }
    }
}