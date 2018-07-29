using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC5Course
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}"); //有符合排除條件時，把request丟回去給iis處理

            routes.MapMvcAttributeRoutes(); //允許屬性路由

            //註:route沒有在比對QueryString
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[]
                {
                    //定義命名空間，使跟區域(Area)內在路由Controller時可以分開，避免名稱重複造成的衝突
                    "MVC5Course.Controllers"
                }
            );
        }
    }
}
