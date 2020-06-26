using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace slnQF
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "App", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "UserLog",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "UserLog", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Ventas",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Ventas", action = "VentasEntrega", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Produccion",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Produccion", action = "Produccion", id = UrlParameter.Optional }
           );

 
            routes.MapRoute(
                          name: "Catalogo",
                          url: "{controller}/{action}/{id}",
                          defaults: new { controller = "Catalogo", action = "Perfiles", id = UrlParameter.Optional }

                      );
            routes.MapRoute(
                name: "Permisos",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Catalogo", action = "Permisos", id = UrlParameter.Optional }

            );
            routes.MapRoute(
                name: "Usuario",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Usuario", action = "Usuario", id = UrlParameter.Optional }

            );
        }
    }
}
