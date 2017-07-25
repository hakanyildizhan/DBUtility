using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBUtility.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private DBUtilityContext db = new DBUtilityContext();

        public ActionResult Index()
        {

            return View();
        }
    }
}