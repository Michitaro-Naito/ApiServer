using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApiServer.Models;

namespace ApiServer.Controllers
{
    public class BannedIdController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: /BannedId/
        public ActionResult Index()
        {
            return View(db.BannedIds.ToList());
        }

        // GET: /BannedId/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannedId bannedid = db.BannedIds.Find(id);
            if (bannedid == null)
            {
                return HttpNotFound();
            }
            return View(bannedid);
        }

        // GET: /BannedId/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /BannedId/Create
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="UserId")] BannedId bannedid)
        {
            if (ModelState.IsValid)
            {
                db.BannedIds.Add(bannedid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bannedid);
        }

        // GET: /BannedId/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannedId bannedid = db.BannedIds.Find(id);
            if (bannedid == null)
            {
                return HttpNotFound();
            }
            return View(bannedid);
        }

        // POST: /BannedId/Edit/5
        // 過多ポスティング攻撃を防止するには、バインド先とする特定のプロパティを有効にしてください。
        // 詳細については、http://go.microsoft.com/fwlink/?LinkId=317598 を参照してください。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="UserId")] BannedId bannedid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bannedid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bannedid);
        }

        // GET: /BannedId/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BannedId bannedid = db.BannedIds.Find(id);
            if (bannedid == null)
            {
                return HttpNotFound();
            }
            return View(bannedid);
        }

        // POST: /BannedId/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BannedId bannedid = db.BannedIds.Find(id);
            db.BannedIds.Remove(bannedid);
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
