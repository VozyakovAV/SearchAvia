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
        public DateTime DateBack { get; set; }

        public abstract bool IsSearching { get; protected set; }
        public abstract void Start();
        public abstract string GetResult();
    }

    public class SearchResult
    {
        public string Airline { get; set; }
        public string[] Flights { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateBack { get; set; }
    }
}