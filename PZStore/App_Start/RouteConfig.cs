using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PZStore
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: null,
               url: "",
               defaults: new { controller = "Home", action = "Index", category = (string)null, productPage = 1 }
            );

            routes.MapRoute(
                name: null,
                url: "Page{productPage}",
                defaults: new { controller = "Home", action = "Index", category = (string)null },
                constraints: new { productPage = @"\d+" } // \d+ regex means that range from 1 and up
            );

            routes.MapRoute(
                name: null,
                url: "{category}",
                defaults: new { controller = "Home", action = "Index", productPage = 1 }
            );

            routes.MapRoute(
                name: null,
                url: "{category}/Page{productPage}",
                defaults: new { controller = "Home", action = "Index" },
                constraints: new { productPage = @"\d+" } // \d+ regex means that range from 1 and up
            );

            routes.MapRoute(
                name: null,
                url: "{controller}/{action}"
            );
        }
    }
}
