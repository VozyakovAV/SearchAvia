using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchAvia
{
    public abstract class SearchBase
    {
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DateBack { get; set; }

        public abstract SearchResult Load();

        public SearchBase(string cityFrom, string cityTo, DateTime date, DateTime dateBack)
        {
            this.CityFrom = cityFrom;
            this.CityTo = cityTo;
            this.Date = date;
            this.DateBack = dateBack;
        }
    }

    public class SearchResult
    {
        public string Airline { get; set; }
        public double Price { get; set; }
        public string[] Flights { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateBack { get; set; }
        public string Url { get; set; }
    }
}