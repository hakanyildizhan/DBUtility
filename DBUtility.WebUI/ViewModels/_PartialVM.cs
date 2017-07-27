using MvcContrib.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DBUtility.WebUI.ViewModels
{
    public enum PartialViewType
    {
        _SelectFile,
        _SelectApplication
    }

    public class _PartialVM
    {
        public string Direction { get; set; }
        public bool CanGoBack { get; set; }
        public bool CanGoNext { get; set; }
        public PartialViewType PartialViewType { get; set; }
    }

    public class CustomModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType.Name.Equals(typeof(_PartialVM).Name))
            {
                var modelTypeValue = controllerContext.Controller.ValueProvider.GetValue(typeof(PartialViewType).Name);
                if (modelTypeValue == null)
                    throw new Exception("View type could not be retrieved. Make sure to include the view type enumeration value on the related partial view.");

                var modelTypeName = modelTypeValue.AttemptedValue;

                var type = modelType.Assembly.GetTypes().SingleOrDefault(x => x.IsSubclassOf(modelType) && x.Name == modelTypeName);
                if (type == null)
                    throw new Exception("Partial view type specified in the view does not match any concrete type. You must create the class first.");

                var concreteInstance = Activator.CreateInstance(type);

                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => concreteInstance, type);

                return concreteInstance;

            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }


        //public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        //{
        //    if (bindingContext.ModelType == typeof(_PartialVM))
        //    {
        //        var modelTypeValue = controllerContext.Controller.ValueProvider.GetValue("PartialViewType");
        //        HttpRequestBase request = controllerContext.HttpContext.Request;

        //        string viewName = request.Form.Get("ViewName");

        //        switch (viewName)
        //        {
        //            case "_SelectFile": return new _SelectFile();
        //            case "_SelectApplication": return new _SelectApplication();
        //            default:
        //                break;
        //        }
        //    }
        //    return null;
        //}
    }

    #region Old binder

    //public class PartialViewModelBinder : DefaultModelBinder
    //{
    //    protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
    //    {
    //        PartialViewType personType = GetValue<PartialViewType>(bindingContext, "PartialViewType");

    //        Type model = _PartialVM.SelectFor(personType);

    //        _PartialVM instance = (_PartialVM)base.CreateModel(controllerContext, bindingContext, model);

    //        bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, model);

    //        return instance;
    //    }

    //    private T GetValue<T>(ModelBindingContext bindingContext, string key)
    //    {
    //        ValueProviderResult valueResult = bindingContext.ValueProvider.GetValue(key);

    //        bindingContext.ModelState.SetModelValue(key, valueResult);

    //        return (T)valueResult.ConvertTo(typeof(T));
    //    }
    //}
    #endregion

}