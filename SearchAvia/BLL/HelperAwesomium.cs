using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Awesomium.Core;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace SearchAvia
{
    public class HelperAwesomium
    {
        private WebView _browser;

        private static HelperAwesomium _instance;
        public static HelperAwesomium Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new HelperAwesomium();
                return _instance;
            }
        }

        public HelperAwesomium()
        {
            Init();
        }

        public string Load(string url, Func<string, bool> checkLoading)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string html = null;
            bool isLoaded = false;
            CreateBrowser();
            SetUrl(url);
            while (!isLoaded && sw.Elapsed.TotalSeconds < 120)
            {
                Wait();
                html = GetHtml();
                isLoaded = checkLoading.Invoke(html);
            }

            while (!isLoaded)
                Thread.Sleep(100);
            return html;
        }

        public string GetHtml()
        {
            string html = null;
            _context.Post(state =>
            {
                if (_browser == null || _browser.IsLoading)
                html = "";
            else
            {
                var t = _browser.ExecuteJavascriptWithResult("document.body.innerHTML");
                html = t == null ? "" : t.ToString();
            }
            }, null);
            while (html == null)
                Thread.Sleep(100);
            return html;
        }


        SynchronizationContext _context = null;
        private void Init()
        {
            _context = null;
            Thread awesomiumThread = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                WebCore.Started += (s, e) => {
                    _context = SynchronizationContext.Current;
                };

                WebCore.Run();
            }));

            awesomiumThread.Start();

            WebConfig.Default.UserAgent = "Chrome";
            WebCore.Initialize(WebConfig.Default);

            while (_context == null)
                Thread.Sleep(100);
        }



        private void CreateBrowser()
        {
            _browser = null;
            _context.Post(state =>
            {
                _browser = WebCore.CreateWebView(1100, 600);
            }, null);
            while (_browser == null)
                Thread.Sleep(100);
        }

        private void SetUrl(string url)
        {
            bool b = false;
            _context.Post(state =>
            {
                _browser.Source = new Uri(url);
                b = true;
            }, null);
            while (!b)
                Thread.Sleep(100);
        }

        private void Wait()
        {
            bool b = false;
            _context.Post(state =>
            {
                WebCore.Update();
                Thread.Sleep(100);
                b = true;
            }, null);

            while (!b)
                Thread.Sleep(100);
        }

        public void OnClick(string selector)
        {
            bool b = false;
            _context.Post(state =>
            {
                _browser.ExecuteJavascript("function AddClickMethod(element, event) {var e = document.createEvent('HTMLEvents'); e.initEvent(event, true, false); element.dispatchEvent(e); } AddClickMethod(document.querySelector('" + selector + "'), 'click');");
                b = true;
            }, null);

            while (!b)
                Thread.Sleep(100);
        }
    }
}
 