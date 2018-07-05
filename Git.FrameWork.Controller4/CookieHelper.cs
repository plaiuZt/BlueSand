using Git.Framework.Resource;
using System;
using System.Web;

namespace Git.Framework.Controller
{

    public class CookieHelper
    {
        public static void Add(string name, string value)
        {
            CookieEntity cookieEntity = ResourceManager.GetCookieEntity(name);
            HttpCookie cookie = new HttpCookie(name, value);
            if (cookieEntity.Expires > 0L)
            {
                cookie.Expires = DateTime.Now.AddSeconds((double) cookieEntity.Expires);
            }
            else
            {
                cookie.Expires = DateTime.MaxValue;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        public static string Get(string name)
        {
            if (HttpContext.Current.Request.Cookies[name] != null)
            {
                return HttpContext.Current.Request.Cookies[name].Value;
            }
            return string.Empty;
        }

        public static void Remove(string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1.0);
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }

        public static void Replace(string name, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
            {
                cookie.Value = value;
                HttpContext.Current.Response.AppendCookie(cookie);
            }
        }
    }
}

