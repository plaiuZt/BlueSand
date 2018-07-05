using Git.Framework.Json;
using System;
using System.Web.Mvc;

namespace Git.Framework.Controller.Mvc
{
    public class JsonBinder<T> : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            string json = controllerContext.HttpContext.Request.Form[bindingContext.ModelName];
            return ConvertJson.GetObjectJson(json, typeof(T));
        }
    }
}

