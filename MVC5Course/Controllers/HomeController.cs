using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        //[ValidateInput(false)] //允許Post過來的內容包含HTML Tag 與 javascript
        //[OutputCache] //Cache該頁面 (用法同webform的OutputCache)
        [ViewBagMessage]
        public ActionResult About()
        {
            //(改以Action filter實作)
            //ViewBag.Message = "Your application description page.123";

            return View();
        }
        
        [LocalOnly]
        //[ViewBagMessage]
        //[LocalOnly,ViewBagMessage] 
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}