using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeeksForLess_test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Themes");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Themes()
        {
            return View("~/Views/Themes/Index.cshtml");
        }

        public ActionResult About()
        {
            ViewBag.Message = "GeeksForLess";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "";

            return View();
        }
    }
}