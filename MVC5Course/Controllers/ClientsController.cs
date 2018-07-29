using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Course.Models;

namespace MVC5Course.Controllers
{
    [RoutePrefix("clients")]
    public class ClientsController : BaseController
    {
        //private FabricsEntities db = new FabricsEntities();

        ClientRepository repo;
        OccupationRepository occuRepo;

        public ClientsController()
        {
            repo = RepositoryHelper.GetClientRepository();
            occuRepo = RepositoryHelper.GetOccupationRepository(repo.UnitOfWork); //occuRepo直接引用repo的UnitOfWork物件 --> 可達到共用一個FabricsEntities的結果 ( ClientRepository GetClientRepository() > GetUnitOfWork() > return new EFUnitOfWork(); ) 
        }

        [Route("")]
        // GET: Clients
        public ActionResult Index()
        {
            ViewBag.keyword = "Hello World"; //從Controller給ViewBag的keyword屬性一個預設值(ViewBag是動態型別，且在view中已經有定義一個表單物件名為keyword) 

            //實作下拉式選單選項(取得選項內容)
            var items = (from p in db.Client
                         select p.CreditRating)
                         .Distinct()
                         .OrderBy(p => p)
                         .Select(p => new SelectListItem()
                         {
                             Text = p.Value.ToString(),
                             Value = p.Value.ToString()
                         });

            ViewBag.CreditRating = new SelectList(items, "Value", "Text"); //會自動綁到名為CreditRating的DropDownList物件


            //var client = db.Client.Include(c => c.Occupation);
            var client = repo.All();
            return View(client.Take(50));
        }

        //(實作批次更新)
        [HttpPost]
        [Route("BatchUpdate")]
        [HandleError(ExceptionType = typeof(DbEntityValidationException), View = "Error_DbEntityValidationException")] //例外過濾器
        public ActionResult BatchUpdate(ClientBatchVM[] data, PageCondVM page) //ClientBatchVM為設計好的ViewModel
        {
            //ModelBinding成功時
            if (ModelState.IsValid)
            {
                //取得更新欄位
                foreach (var vm in data)
                {
                    var client = db.Client.Find(vm.ClientId);
                    client.FirstName = vm.FirstName;
                    client.MiddleName = vm.MiddleName;
                    client.LastName = vm.LastName;
                }

                db.SaveChanges(); //更新資料庫

                //(改以HandleError實作)
                //try
                //{
                //    db.SaveChanges(); //更新資料庫
                //}
                //catch(DbEntityValidationException ex)
                //{
                //    List<string> errors = new List<string>();
                //    foreach(var vError in ex.EntityValidationErrors)
                //    {
                //        foreach(var err in vError.ValidationErrors)
                //        {
                //            errors.Add(err.PropertyName + ": " + err.ErrorMessage);
                //        }
                //    }

                //    return Content(string.Join(", ", errors.ToArray()));

                //}

                return RedirectToAction("Index"); //導引至Index頁
            }

            //ModelBinding失敗時 --> 重新查詢資料
            ViewData.Model = repo.All().Take(10);

            return View("Index");
        }

        [Route("search")]
        //實作一個關鍵字搜尋功能
        public ActionResult Search(string keyword, string CreditRating)
        {
            //var client = db.Client.AsQueryable();

            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    //client = client.Where(p => p.FirstName.Contains(keyword));
            //    client = client.Where(p => p.FirstName.Contains(keyword)).Take(5); //將搜尋結果分頁
            //}

            ////return View(client); //指定於Search這個view呈現資料(預設)
            //return View("Index", client); //指定於Index這個view呈現資料

            var client = repo.搜尋名稱(keyword);

            //實作下拉式選單選項 (保留選項及選取狀態) ※新增一個CreditRating參數以進行ModelBinding
            var items = (from p in db.Client
                         select p.CreditRating)
                         .Distinct()
                         .OrderBy(p => p);

            ViewBag.CreditRating = new SelectList(items);


            return View("Index", client);
        }

        [Route("{id}")]
        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            Client client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        //(實作在Detail頁面顯示Order內容，新增View OrderList (Template為List Model為Order)並在Detials.cshtml以@Html.Action顯示)
        [Route("{id}/orders")]
        [ChildActionOnly] //限制此Action不能被單獨瀏覽 ex: localhost:XXXXX/Clients/10/orders 在未限制前可以直接瀏覽，一但限制後會出現「System.InvalidOperationException: 動作 'Details_OrderList' 只能由子要求存取。」...的錯誤
        public ActionResult Details_OrderList(int id)
        {
            ViewData.Model = repo.Find(id).Order.ToList();
            return PartialView("OrderList");
        }


        //(示範屬性路由 catch all *以後的字串皆視為一個參數  ex John/Tom/May...)
        [Route("{*name}")]
        public ActionResult Detail2(string name)
        {
            string[] names = name.Split('/');
            string FirstName = names[0];
            string MiddleName = names[1];
            string LastName = names[2];

            Client client = repo.All().FirstOrDefault(p => p.FirstName == FirstName && p.MiddleName == MiddleName && p.LastName == LastName);

            if (client == null)
            {
                return HttpNotFound();
            }
            return View("Details", client);
        }

        [Route("create")]
        // GET: Clients/Create
        public ActionResult Create()
        {
            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName");
            //return View();

            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("create")]
        public ActionResult Create([Bind(Include = "ClientId,FirstName,MiddleName,LastName,Gender,DateOfBirth,CreditRating,XCode,OccupationId,TelephoneNumber,Street1,Street2,City,ZipCode,Longitude,Latitude,Notes,IdNumber")] Client client)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Client.Add(client);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            //return View(client);

            if (ModelState.IsValid)
            {
                repo.Add(client);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Edit/5
        [Route("edit/{id}")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            Client client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            //return View(client);
            
            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        //(練習透過 TryUpdateModel 實現延遲驗證 不用再額外判斷ModelState.IsValid的值)
        //(改寫MVC的預先ModelBinding為延遲ModelBinding ※不使用ViewModel的另一個作法)
        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("edit/{id}")]
        public ActionResult Edit(int id, FormCollection form)
        {
            var client = repo.Find(id);

            //(※FormCollection物件 不會產生ModelState內容；在此範例中定義型別為FormCollection 的form參數亦不使用，只是預留)
            //延遲Binding (ModelBinding 同時也會進行輸入、模型驗證等)
            if (TryUpdateModel(client, "", null, new string[] { "FirstName" }))
            {
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            //if (ModelState.IsValid)
            //{
            //    db.Entry(client).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //ViewBag.OccupationId = new SelectList(db.Occupation, "OccupationId", "OccupationName", client.OccupationId);
            //return View(client);

            if (ModelState.IsValid)
            {
                //不建議的做法(因為所有欄位都會被更新) --> 建議改為ViewModel的方式處理
                var db = repo.UnitOfWork.Context;
                db.Entry(client).State = EntityState.Modified;

                db.SaveChanges(); //亦會產生一筆交易(失敗時會自動執行rollback)
                return RedirectToAction("Index");
            }
            
            ViewBag.OccupationId = new SelectList(occuRepo.All(), "OccupationId", "OccupationName", client.OccupationId);
            return View(client);
        }

        // GET: Clients/Delete/5
        [Route("delete/{id}")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Client client = db.Client.Find(id);
            Client client = repo.Find(id.Value);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("delete/{id}")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Client client = db.Client.Find(id);
            //db.Client.Remove(client);
            //db.SaveChanges();
            //return RedirectToAction("Index");

            Client client = repo.Find(id);
            repo.Delete(client);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
