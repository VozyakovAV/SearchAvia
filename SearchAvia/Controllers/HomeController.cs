﻿using System;
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
            var t = new SearchAviasales();
            t.Start();
            return View();
        }
    }
}