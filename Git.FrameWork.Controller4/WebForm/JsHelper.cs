namespace Git.Framework.Controller.WebForm
{
    using System;
    using System.Web;
    using System.Web.UI;

    public class JsHelper
    {
        public static void Alert(string message)
        {
            string format = "<script language=javascript>alert('{0}');</script>";
            HttpContext.Current.Response.Write(string.Format(format, message));
        }

        public static void AlertAndGoHistory(string message, int value)
        {
            string format = "<Script language='JavaScript'>alert('{0}');history.go({1});</Script>";
            HttpContext.Current.Response.Write(string.Format(format, message, value));
            HttpContext.Current.Response.End();
        }

        public static void AlertAndParentUrl(string message, string toURL)
        {
            string format = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(format, message, toURL));
        }

        public static void AlertAndRedirect(string message, string toURL)
        {
            string format = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(format, message, toURL));
            HttpContext.Current.Response.End();
        }

        public static void BackHistory(int value)
        {
            string format = "<Script language='JavaScript'>history.go({0});</Script>";
            HttpContext.Current.Response.Write(string.Format(format, value));
            HttpContext.Current.Response.End();
        }

        public static void ParentRedirect(string ToUrl)
        {
            string format = "<script language=javascript>window.top.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(format, ToUrl));
        }

        public static void Redirect(string toUrl)
        {
            string format = "<script language=javascript>window.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(format, toUrl));
        }

        public static void RegisterScriptBlock(Page page, string _ScriptString)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "scriptblock", "<script type='text/javascript'>" + _ScriptString + "</script>");
        }
    }
}

