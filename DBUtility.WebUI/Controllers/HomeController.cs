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
            _PartialVM model = new _SelectFile()
            {
                Title = "_SelectFile",
                CanGoBack = false,
                CanGoNext = true
            };

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Navigate(_PartialVM model)
        {
            string currentView = model.Title;
            string direction = model.Direction;

            if (model is _SelectFile)
            {
                string file = (model as _SelectFile).File;
            }
            

            string partialViewToRender = string.Empty;
            bool canGoBack = true;
            bool canGoNext = true;

            if (direction.Equals("Next", StringComparison.InvariantCultureIgnoreCase))
            {
                switch (currentView)
                {
                    case "_SelectFile":
                        partialViewToRender = "_SelectApplication";
                        break;
                    default:
                        break;
                }
            }

            else
            {
                switch (currentView)
                {
                    case "_SelectFile":
                        partialViewToRender = "_SelectFile";
                        break;
                    case "_SelectApplication":
                        partialViewToRender = "_SelectFile";
                        break;
                    default:
                        break;
                }
            }
            

            FadingBodyView newModel = new FadingBodyView()
            {
                Title = partialViewToRender,
                CanGoBack = canGoBack,
                CanGoNext = canGoNext
            };

            return PartialView(partialViewToRender, newModel);
        }
    }
}