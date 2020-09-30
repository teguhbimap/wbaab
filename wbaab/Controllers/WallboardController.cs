using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace wbaab.Controllers
{
    public class WallboardController : Controller
    {
        static string URL_SIGNAL = System.Configuration.ConfigurationManager.AppSettings["urlSignalR"];
        public ActionResult View1()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            return View();
        }
        public ActionResult View2()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            return View();
        }
        public ActionResult View3()
        {
            ViewBag.urlSignalR = URL_SIGNAL;
            return View();
        }
    }
}
