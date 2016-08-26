using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace SearchAvia
{
    // https://search.aviasales.ru/MOW2608PAR19091

    public class SearchAviasales : SearchBase
    {
        public override bool IsSearching
        {
            get
            {
                throw new NotImplementedException();
            }

            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override string GetResult()
        {
            throw new NotImplementedException();
        }

        public override void Start()
        {
            //var html = LoadContent();
            //File.WriteAllText(@"C:\temp\2\1.html", html);
            var html = File.ReadAllText(@"C:\temp\2\1.html");
            Parse(html);
        }

        private string LoadContent()
        {
            var url = "https://search.aviasales.ru/MOW2608PAR19091";
            var loader = new HelperAwesomium();
            loader.Load(url, CheckLoading);
            return loader.GetHtml();
        }

        private bool CheckLoading(string html)
        {
            return html.Contains("ticket-new");
        }

        private void Parse(string html)
        {

        }
    }
}