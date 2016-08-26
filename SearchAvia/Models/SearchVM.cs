using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SearchAvia
{
    public class SearchVM
    {
        public string CityFrom { get; set; }
        public string CityTo { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateBack { get; set; }

        public SearchVM()
        {
            this.CityFrom = "MOW";
            this.CityTo = "PAR";
            this.Date = DateTime.Now.AddDays(1).Date;
            this.DateBack = DateTime.Now.AddDays(7).Date;
        }
    }
}