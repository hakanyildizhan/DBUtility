using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBUtility.WebUI.ViewModels
{
    public class _PartialVM
    {
        public string Title { get; set; }
        public string Direction { get; set; }
        public bool CanGoBack { get; set; }
        public bool CanGoNext { get; set; }

        public PartialViewType PartialViewType { get; set; }

        internal static Type SelectFor(PartialViewType type)
        {
            switch (type)
            {
                case PartialViewType._SelectFile:
                    return typeof(_SelectFile);
                case PartialViewType._SelectApplication:
                    return typeof(_SelectApplication);
                default:
                    throw new NotImplementedException();
            }
        }

    }
    
    public class PartialViewModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            PartialViewType personType = GetValue<PartialViewType>(bindingContext, "PartialViewType");

            Type model = _PartialVM.SelectFor(personType);

            _PartialVM instance = (_PartialVM)base.CreateModel(controllerContext, bindingContext, model);

            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, model);

            return instance;
        }

        private T GetValue<T>(ModelBindingContext bindingContext, string key)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(key);

            bindingContext.ModelState.SetModelValue(key, valueResult);

            return (T)valueResult.ConvertTo(typeof(T));
        }
    }

    public enum PartialViewType
    {
        _SelectFile,
        _SelectApplication
    }
}