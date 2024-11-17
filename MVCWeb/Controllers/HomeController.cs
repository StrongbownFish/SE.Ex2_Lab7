using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Item()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Agent()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Order()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}