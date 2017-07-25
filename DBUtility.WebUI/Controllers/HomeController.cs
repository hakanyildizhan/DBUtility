using DBUtility.WebUI.ViewModels;
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

        [HttpGet]
        public ActionResult Index()
        {
            FadingBodyView model = new FadingBodyView()
            {
                Title = "_SelectFile",
                CanGoBack = false,
                CanGoNext = true
            };

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Next(string currentView, string file)
        {
            string partialViewToRender = string.Empty;
            bool canGoBack = true;
            bool canGoNext = true;

            switch (currentView)
            {
                case "_SelectFile":
                    partialViewToRender = "_SelectApplication";
                    break;
                default:
                    break;
            }

            FadingBodyView model = new FadingBodyView()
            {
                Title = partialViewToRender,
                CanGoBack = canGoBack,
                CanGoNext = canGoNext
            };

            return PartialView(partialViewToRender, model);
        }
    }
}