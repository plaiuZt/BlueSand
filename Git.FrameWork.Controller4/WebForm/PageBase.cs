using Git.Framework.Io;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;

namespace Git.Framework.Controller.WebForm
{
    public class PageBase : Page
    {
        private void InitPath()
        {
            this.Url = base.Request.Url.ToString();
            this.ReferrerUrl = (base.Request.UrlReferrer != null) ? base.Request.UrlReferrer.ToString() : string.Empty;
            this.ServerPath = base.Request.AppRelativeCurrentExecutionFilePath;
            this.Path = base.Request.FilePath;
            this.PhysicPath = base.Request.PhysicalPath;
            this.DomainPath = base.Request.PhysicalApplicationPath;
            this.FileDirectory = FileManager.GetFileDirectory(this.PhysicPath);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.InitPath();
        }

        protected string DomainPath { get; set; }

        protected string FileDirectory { get; set; }

        protected string Path { get; set; }

        protected string PhysicPath { get; set; }

        protected string ReferrerUrl { get; set; }

        public static string RootPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/");
            }
        }

        protected string ServerPath { get; set; }

        protected string Url { get; set; }

        public static string UserIP
        {
            get
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
        }

        public string UserLanguage
        {
            get
            {
                if ((base.Request.UserLanguages != null) && (base.Request.UserLanguages.Length > 0))
                {
                    return base.Request.UserLanguages[0];
                }
                return base.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];
            }
        }
    }
}

