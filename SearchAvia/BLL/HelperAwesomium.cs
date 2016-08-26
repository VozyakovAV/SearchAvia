using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Awesomium.Core;
using System.Threading;

namespace SearchAvia
{
    public class HelperAwesomium
    {
        private WebView _browser;
        public bool IsLoading { get; protected set; }

        public HelperAwesomium()
        {
            _browser = WebCore.CreateWebView(1100, 600);
        }

        public void Load(string url)
        {
            IsLoading = true;
            Init();
            _browser.Source = new Uri(url);
            while (IsLoading)
            {
                WebCore.Update();
                Thread.Sleep(200);
            }
        }

        public void Stop()
        {
            IsLoading = false;
        }

        public string GetHtml()
        {
            return _browser == null ? "" : _browser.HTML;
        }

        private static object _lock = new object();
        private void Init()
        {
            lock (_lock)
            {
                if (!WebCore.IsInitialized)
                    WebCore.Initialize(WebConfig.Default);
            }
        }
    }
}