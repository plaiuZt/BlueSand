using System.Web.Mvc;

namespace Git.Storage.Web.Areas.Biz
{
    public class BizAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Biz";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Biz_default",
                "Biz/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
