using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private static StatusSearchVM _status;
        public StatusSearchVM Status
        {
            get
            {
                if (_status == null)
                    _status = new StatusSearchVM();
                return _status;
            }
        }

        [HttpPost]
        public JsonResult Search(SearchVM search)
        {
            if (!Status.IsSearching)
            {
                Status.IsSearching = true;
                Status.Message = null;
                Status.Result = null;
                Task.Run(() => StartSearch(search));
            }
            return Json(Status);
        }

        public JsonResult GetStatusSearch()
        {
            return Json(Status);
        }

        private void StartSearch(SearchVM search)
        {
            try
            {
                var s = new SearchAviasales(search.CityFrom, search.CityTo, search.Date, search.DateBack);
                var result = s.Load();
                Status.Result = new SearchResultVM(result);
            }
            catch (Exception ex)
            {
                Status.Result = null;
                Status.Message = ex.Message;
            }
            finally
            {
                Status.IsSearching = false;
            }
        }
    }
}