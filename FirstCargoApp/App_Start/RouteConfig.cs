using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FirstCargoApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Login", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ManageUser",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ManageUser", action = "ManageUser", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Vehicule",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Vehicule", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Package",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Package", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Orther",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Other", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "PrintOrders",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Vehicule", action = "printGeneralReport", id = UrlParameter.Optional }
            );

        }
    }
}
