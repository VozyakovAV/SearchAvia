using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Awesomium.Core;
using System.Threading;
using System.Threading.Tasks;

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
            _browser = WebCore.CreateWebView(1100, 600);
            _browser.Source = new Uri(_url);
            while (IsLoading)
            {
                WebCore.Update();
                Thread.Sleep(100);
                var b = _checkLoading.Invoke(_browser.HTML);
                if (b)
                    IsLoading = false;
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