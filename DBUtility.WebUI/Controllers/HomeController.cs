using DBUtility.WebUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBUtility.Core;

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
                CanGoBack = false,
                CanGoNext = true
            };

            return View(model);
        }

        [HttpPost]
        public PartialViewResult Navigate([ModelBinder(typeof(CustomModelBinder))] _PartialVM model)
        {
            string currentView = model.PartialViewType.ToString();
            string direction = model.Direction;

            // parse values view by view
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
                PartialViewType = partialViewToRender.ToEnum<PartialViewType>(),
                CanGoBack = canGoBack,
                CanGoNext = canGoNext
            };
            
            return PartialView(partialViewToRender, model);
        }
    }
}