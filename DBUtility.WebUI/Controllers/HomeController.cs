using DBUtility.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBUtility.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private DBUtilityContext db = new DBUtilityContext();
        private static Dictionary<string, int> _viewOrder;

        static HomeController()
        {
            _viewOrder = new Dictionary<string, int>();
            _viewOrder.Add("_SelectFile", 0);
            _viewOrder.Add("_SelectApplication", 1);
        }

        [HttpGet]
        public ActionResult Index()
        {
            _PartialVM model = new _SelectFile()
            {
                ViewName = "_SelectFile",
                CanGoBack = false,
                CanGoNext = true
            };

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Navigate(_PartialVM model)
        {
            string currentView = model.ViewName;
            string direction = model.Direction;

            if (model is _SelectFile)
            {
                string file = (model as _SelectFile).File;
            }
            
            string partialViewToRender = string.Empty;
            bool canGoBack = true;
            bool canGoNext = true;

            int currentViewOrder = _viewOrder[currentView];

            partialViewToRender = direction.Equals("Next", StringComparison.InvariantCultureIgnoreCase) ?
                _viewOrder.FirstOrDefault(x => x.Value.Equals(currentViewOrder + 1)).Key
                :
                _viewOrder.FirstOrDefault(x => x.Value.Equals(currentViewOrder - 1)).Key;

            if (currentViewOrder.Equals(0)) canGoBack = false;
            else if (_viewOrder.Count.Equals(_viewOrder[currentView] + 1)) canGoNext = false;

            model = new _PartialVM()
            {
                ViewName = partialViewToRender,
                CanGoBack = canGoBack,
                CanGoNext = canGoNext
            };


            return PartialView(partialViewToRender, (model as _PartialVM));
        }
    }
}