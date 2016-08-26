using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SearchAvia.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new SearchVM();
            return View(vm);
        }

        [HttpPost]
        public JsonResult Search(SearchVM search)
        {
            SearchResult res = null;
            string msg = null;

            try
            {
                var s = new SearchAviasales(search.CityFrom, search.CityTo, search.Date, search.DateBack);
                res = s.Load();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            return Json(new {
                result = res,
                msg
            });
        }
    }
}