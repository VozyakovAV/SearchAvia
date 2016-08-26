using System;
using System.Collections.Generic;
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
            var html = LoadContent();

            
        }

        private string LoadContent()
        {
            var url = "https://search.aviasales.ru/MOW2608PAR19091";
            var loader = new HelperAwesomium();
            loader.Load(url);
            while (!loader.GetHtml().Contains("ticket-new"))
            {
                Thread.Sleep(200);
            }
            loader.Stop();
            return loader.GetHtml();
        }
    }
}