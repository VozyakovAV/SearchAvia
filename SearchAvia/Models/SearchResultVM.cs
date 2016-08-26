using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchAvia
{
    public class SearchResultVM
    {
        public string Airline { get; set; }
        public string Price { get; set; }
        public string Flights { get; set; }
        public string Date { get; set; }
        public string DateBack { get; set; }
        public string Url { get; set; }

        public SearchResultVM(SearchResult result)
        {
            this.Airline = result.Airline;
            this.Price = result.Price.ToString();
            this.Date = result.Date.ToString("dd.MM.yyyy HH:mm");
            this.DateBack = result.DateBack.ToString("dd.MM.yyyy HH:mm");
            this.Url = result.Url;
        }
    }
}