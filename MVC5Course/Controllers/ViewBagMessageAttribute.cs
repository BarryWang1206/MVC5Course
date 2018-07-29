using System;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ViewBagMessageAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewBag.Message = "Your application description page.123";

            base.OnActionExecuting(filterContext);
        }

    }
}