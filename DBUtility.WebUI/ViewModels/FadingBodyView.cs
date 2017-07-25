using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DBUtility.WebUI.ViewModels
{
    public class FadingBodyView
    {
        public string Title { get; set; }
        public bool CanGoBack { get; set; }
        public bool CanGoNext { get; set; }
    }
}