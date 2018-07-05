using Git.Framework.Controller;
using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Io;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using System.Web.Routing;

namespace Git.Framework.Controller.Mvc
{
    public class ControllerBase : System.Web.Mvc.Controller
    {
        protected string GetAreaName()
        {
            string str = string.Empty;
            if (base.RouteData.Values.Count > 0)
            {
                if (base.RouteData.Values.ContainsKey("area"))
                {
                    str = base.RouteData.Values["area"].ToString();
                }
                if (string.IsNullOrEmpty(str))
                {
                    str = base.RouteData.DataTokens["area"].ToString();
                }
            }
            return str;
        }

        protected object GetRouteValue(string key)
        {
            if ((!base.RouteData.IsNull() && (base.RouteData.Values != null)) && !base.RouteData.Values[key].IsNull())
            {
                return base.RouteData.Values[key];
            }
            return WebUtil.GetQueryStringValue<string>(key);
        }

        protected T GetRouteValue<T>(string key)
        {
            if ((!base.RouteData.IsNull() && (base.RouteData.Values != null)) && !base.RouteData.Values[key].IsNull())
            {
                return ConvertHelper.ToType<T>(base.RouteData.Values[key].ToString(), default(T));
            }
            return WebUtil.GetQueryStringValue<T>(key, default(T));
        }

        protected T GetRouteValue<T>(string key, T defaultValue)
        {
            if ((!base.RouteData.IsNull() && (base.RouteData.Values != null)) && !base.RouteData.Values[key].IsNull())
            {
                return ConvertHelper.ToType<T>(base.RouteData.Values[key].ToString(), defaultValue);
            }
            return WebUtil.GetQueryStringValue<T>(key, defaultValue);
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            this.InitPath(requestContext);
        }

        private void InitPath(RequestContext requestContext)
        {
            this.Url = requestContext.HttpContext.Request.Url.ToString();
            this.ReferrerUrl = (requestContext.HttpContext.Request.UrlReferrer != null) ? requestContext.HttpContext.Request.UrlReferrer.ToString() : string.Empty;
            this.ServerPath = requestContext.HttpContext.Request.AppRelativeCurrentExecutionFilePath;
            this.Path = requestContext.HttpContext.Request.FilePath;
            this.PhysicPath = requestContext.HttpContext.Request.PhysicalPath;
            this.DomainPath = requestContext.HttpContext.Request.PhysicalApplicationPath;
            this.FileDirectory = FileManager.GetFileDirectory(this.PhysicPath);
            this.IP = base.Request.UserHostAddress;
            this.HostName = base.Request.UserHostName;
            string str = string.Empty;
            string requiredString = string.Empty;
            string str3 = string.Empty;
            if (requestContext.RouteData.Values.Count > 0)
            {
                if (requestContext.RouteData.Values.ContainsKey("area"))
                {
                    str = requestContext.RouteData.Values["area"].ToString();
                }
                if (string.IsNullOrEmpty(str) && requestContext.RouteData.DataTokens.ContainsKey("area"))
                {
                    str = requestContext.RouteData.DataTokens["area"].ToString();
                }
                requiredString = requestContext.RouteData.GetRequiredString("controller");
                str3 = requestContext.RouteData.Values["action"].ToString();
            }
            else
            {
                requiredString = "Home";
                str3 = "Index";
            }
            str = (("root" == str.ToLower()) || str.IsEmpty()) ? string.Empty : ("/" + str);
            string[] textArray1 = new string[] { str, "/", requiredString, "/", str3 };
            this.RequestPath = string.Concat(textArray1);
        }

        public string DomainPath { get; set; }

        public string FileDirectory { get; set; }

        public string HostName { get; set; }

        public string IP { get; set; }

        public string Path { get; set; }

        public string PhysicPath { get; set; }

        public string ReferrerUrl { get; set; }

        public string RequestPath { get; set; }

        public string ServerPath { get; set; }

        public string Url { get; set; }
    }
}

