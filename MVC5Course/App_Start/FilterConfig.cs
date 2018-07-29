using System.Web;
using System.Web.Mvc;

namespace MVC5Course
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //所有的Action發生例外時都會觸發
            filters.Add(new HandleErrorAttribute());
        }
    }
}
