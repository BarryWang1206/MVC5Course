using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class MBController : BaseController
    {
        //練習將字串當成model設定給ViewData.Model
        public ActionResult Index()
        {
            var data = "Hello World";
            ViewData.Model = data;
            return View();
        }

        //練習使用ViewBag
        public ActionResult ViewBagDemo()
        {
            ViewBag.Text = "Hi";
            ViewBag.Word = "Is Word";
            ViewBag.GG = "you are GG";

            ViewData["Data"] = db.Client.Take(10).ToList();

            return View();
        }

        //練習使用ViewData (為弱型別)
        public ActionResult ViewDataDemo()
        {
            ViewData["Text"] = "Hi";
            return View();
        }

        //練習使用TempData (此處練習存值到TempData後轉址)
        public ActionResult TempDataSave()
        {
            TempData["Text"] = "Temp"; //給值 (※TempData的值在讀取一次後將會被清除，實際上該值是存在server 的 session)
            return RedirectToAction("TempDataDemo"); //轉址
        }

        //練習使用TempData (顯示TempData)
        public ActionResult TempDataDemo()
        {
            return View();
        }
        
        //FormCollection 不會產生ModelState (實務上盡量不要用)
        public ActionResult FormTest(FormCollection form)
        {
            return View();
        }
    }
}