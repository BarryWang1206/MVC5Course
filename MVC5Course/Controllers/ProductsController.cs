using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;
using Omu.ValueInjecter;

namespace MVC5Course.Controllers
{
    public class ProductsController : Controller
    {
        private FabricsEntities db = new FabricsEntities(); //DbContext

        // GET: Products
        public ActionResult Index()
        {
            var data = db.Product
                .OrderByDescending(p => p.ProductId)
                .Take(10)
                .ToList();    
            return View(data);
        }

        //(顯示自訂欄位)
        public ActionResult Index2()
        {
            var data = db.Product
            .Where(p => p.Active == true)
            .OrderByDescending(p => p.ProductId)
            .Take(10)
            .Select(p => new ProductViewModel()
            {
                ProductName = p.ProductName,
                ProductId = p.ProductId,
                Price = p.Price,
                Stock = p.Stock
            });

            return View(data);
        }

         //(資料收集頁)
        public ActionResult AddNewProduct()
        {
            return View();
        }

        //(資料收集頁) (負責接資料)
        [HttpPost] //僅在HttpPost時執行此Action
        public ActionResult AddNewProduct(ProductViewModel data)
        {
            if (!ModelState.IsValid)
            {
                //失敗時顯示原本的頁面
                return View();
            }

            //寫入資料至DB的流程
            var product = new Product()
            {
                //ProductId = data.ProductId, //ProductId 在DB中已被定義為識別子(會自動遞增)
                Active = true,
                Price = data.Price,
                Stock = data.Stock,
                ProductName = data.ProductName
            };

            db.Product.Add(product); //新增物件中資料
            db.SaveChanges(); //將物件中資料存入DB

            //成功時重新導向頁面
            return RedirectToAction("Index2");
        }

        //(編輯資料頁)
        public ActionResult EditOne(int id)
        {
            //var data = db.Product.Find(id);
            var data = db.Product.FirstOrDefault(p => p.ProductId == id);

            return View(data);
        }

        //(編輯資料頁) (負責接資料)
        [HttpPost] //僅在HttpPost時執行此Action
        public ActionResult EditOne(int id, ProductViewModel data)
        {
            //確認Model binding的過程其ModelState是否正確
            if (!ModelState.IsValid)
            {
                //失敗時顯示原本的頁面
                return View();
            }

            //更新資料至DB的流程
            var one = db.Product.Find(id);

            //one.InjectFrom(data); //方法二 (需要安裝ValueInjecter套件)

            //方法一
            one.ProductName = data.ProductName;
            one.Stock = data.Stock;
            one.Price = data.Price;

            db.SaveChanges(); //將物件中資料存入DB (更新資料)

            //成功時重新導向頁面
            return RedirectToAction("Index2");
        }

        //(刪除資料頁)
        public ActionResult DeleteOne(int id)
        {
            var item = db.Product.Find(id);
            if(item == null)
            {
                return HttpNotFound();
            }

            //db.Product.Remove(new Product { ProductId = id }); //錯誤作法，因為new出來的Product物件EF無法識別異動的狀況...

            db.Product.Remove(item); //需明確指定要刪除的物件
            db.SaveChanges();

            //return View();
            return RedirectToAction("Index2");
        }

        //--------------------------------------------------------


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,Price,Active,Stock")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
