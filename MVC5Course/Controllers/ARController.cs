using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MVC5Course.Controllers
{
    public class ARController : Controller
    {
        // GET: AR
        public ActionResult Index()
        {
            return View();
        }


        //練習將字串當成 Model 傳到 View 並顯示 Model 內容
        public ActionResult ViewTest()
        {
            string model = "My Data (View)";
            return View((object)model);
        }


        //練習用 PartialView 顯示一個沒有 Layout 的頁面
        public ActionResult PartialtViewTest()
        {
            string model = "My Data (PartialView)";
            return PartialView("ViewTest", (object)model);
        }

        //Content的範例
        public ActionResult ContentTest()
        {
            return Content("Test Content!", "text/plain", Encoding.GetEncoding("Big5"));
        }


        //開啟檔案 下載檔案的範例
        public ActionResult FileTest(string dl) {
            if (string.IsNullOrEmpty(dl))
            {
                return File(Server.MapPath("~/App_Data/fifa.jpg"), "image/jpeg");
            }
            else
            {
                return File(Server.MapPath("~/App_Data/fifa.jpg"), "image/jpeg","dl-fifa.jpg");
            }
        }
            
    }
}