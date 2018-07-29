using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC5Course
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas(); //區域的路由優先
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //將不需要搜尋的WebFormViewEngine移除以加速View頁面被找到的時間
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine()); //只添加RazorViewEngine
        }
    }
}
