using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class RazorController : Controller
    {
        // GET: Razor
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Page1()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }
    }
}