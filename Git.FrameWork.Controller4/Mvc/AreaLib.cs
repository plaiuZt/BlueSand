namespace Git.Framework.Controller.Mvc
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.Routing;

    public static class AreaLib
    {
        public static void CreateArea(this RouteCollection routes, string areaName, string contronllerNamespace, params Route[] routeEntries)
        {
            foreach (Route route in routeEntries)
            {
                if (route.Constraints == null)
                {
                    route.Constraints = new RouteValueDictionary();
                }
                if (route.Defaults == null)
                {
                    route.Defaults = new RouteValueDictionary();
                }
                if (route.DataTokens == null)
                {
                    route.DataTokens = new RouteValueDictionary();
                }
                route.Constraints.Add("area", areaName);
                route.Defaults.Add("area", areaName);
                string[] textArray1 = new string[] { contronllerNamespace };
                route.DataTokens.Add("namespaces", textArray1);
                if (!routes.Contains(route))
                {
                    routes.Add(route);
                }
            }
        }
    }
}

