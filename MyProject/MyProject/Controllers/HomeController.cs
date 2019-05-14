using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute(new { controller = "Main", action = "Index" });
        }

        public ActionResult MyIndex()
        {
            return View();
        }


    }
}

