using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIGN.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                //return Redirect("/account/login");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
        public ActionResult ChungTu()
        {
            return PartialView("~/Views/Home/ChungTu.cshtml");
        }

        public ActionResult XuLyChungTu()
        {
            return PartialView("~/Views/Home/XuLyChungTu.cshtml");
        }
        public ActionResult TraCuuKetQua()
        {
            return PartialView("~/Views/Home/TraCuuKetQua.cshtml");
        }


     

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
