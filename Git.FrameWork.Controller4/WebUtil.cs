using Git.Framework.DataTypes;
using Git.Framework.DataTypes.ExtensionMethods;
using Git.Framework.Io;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace Git.Framework.Controller
{
    public class WebUtil
    {
        public static void DownLoad(string filePath, string saveName)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("未提供可供下载的文件路径！");
            }
            if (!File.Exists(filePath))
            {
                throw new Exception("下载的文件已经被移除！");
            }
            if (string.IsNullOrEmpty(saveName))
            {
                saveName = FileManager.GetItemInfo(filePath).Name;
            }
            FileInfo info = new FileInfo(filePath);
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlPathEncode(saveName));
            HttpContext.Current.Response.AddHeader("Content-Length", info.Length.ToString());
            HttpContext.Current.Response.AddHeader("Content-Transfer-Encoding", "binary");
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("gb2312");
            HttpContext.Current.Response.WriteFile(info.FullName);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        protected string GetAreaName()
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;
            string str = string.Empty;
            if (routeData.Values.Count > 0)
            {
                if (routeData.Values.ContainsKey("area"))
                {
                    str = routeData.Values["area"].ToString();
                }
                if (string.IsNullOrEmpty(str))
                {
                    str = routeData.DataTokens["area"].ToString();
                }
            }
            return str;
        }

        public static T GetFormObject<T>(string key)
        {
            T local = JsonConvert.DeserializeObject<T>(Form[key]);
            return ((local == null) ? default(T) : local);
        }

        public static T GetFormObject<T>(string key, T defaultValue)
        {
            T local = JsonConvert.DeserializeObject<T>(Form[key]);
            return ((local == null) ? defaultValue : local);
        }

        public static T GetFormValue<T>(string key)
        {
            return GetFormValue<T>(key, default(T));
        }

        public static T GetFormValue<T>(string key, T defaultValue)
        {
            return ConvertHelper.ToType<T>(Form[key], defaultValue);
        }

        public static T GetQueryStringValue<T>(string key)
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;
            if ((!routeData.IsNull() && (routeData.Values != null)) && !routeData.Values[key].IsNull())
            {
                return ConvertHelper.ToType<T>(routeData.Values[key].ToString(), default(T));
            }
            return ConvertHelper.ToType<T>(QueryString[key], default(T));
        }

        public static T GetQueryStringValue<T>(string key, T defaultValue)
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;
            if ((!routeData.IsNull() && (routeData.Values != null)) && !routeData.Values[key].IsNull())
            {
                return ConvertHelper.ToType<T>(routeData.Values[key].ToString(), defaultValue);
            }
            return ConvertHelper.ToType<T>(QueryString[key], defaultValue);
        }

        public static object GetRouteValue(string key)
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;
            if ((!routeData.IsNull() && (routeData.Values != null)) && !routeData.Values[key].IsNull())
            {
                return routeData.Values[key];
            }
            return GetQueryStringValue<string>(key);
        }

        public static T GetRouteValue<T>(string key)
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;
            if ((!routeData.IsNull() && (routeData.Values != null)) && !routeData.Values[key].IsNull())
            {
                return ConvertHelper.ToType<T>(routeData.Values[key].ToString(), default(T));
            }
            return GetQueryStringValue<T>(key, default(T));
        }

        public static T GetRouteValue<T>(string key, T defaultValue)
        {
            RouteData routeData = HttpContext.Current.Request.RequestContext.RouteData;
            if ((!routeData.IsNull() && (routeData.Values != null)) && !routeData.Values[key].IsNull())
            {
                return ConvertHelper.ToType<T>(routeData.Values[key].ToString(), defaultValue);
            }
            return GetQueryStringValue<T>(key, defaultValue);
        }

        public static NameValueCollection Form
        {
            get
            {
                return HttpContext.Current.Request.Form;
            }
        }

        public static NameValueCollection QueryString
        {
            get
            {
                return HttpContext.Current.Request.QueryString;
            }
        }
    }
}

