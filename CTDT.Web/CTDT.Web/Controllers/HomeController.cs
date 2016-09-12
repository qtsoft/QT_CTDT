using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CTDT.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EdataList()
        {
            return PartialView("~/Views/Home/_EdataList.cshtml");
        }
    }
}