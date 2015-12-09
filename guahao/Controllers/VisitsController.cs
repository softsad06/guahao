using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using guahao.Models;

namespace guahao.Controllers
{
    public class VisitsController : Controller
    {
        private DB db = new DB();

        // GET: Visits
        public ActionResult Index()
        {
            var visit = db.visit.Include(v => v.doctor);
            return View(visit.ToList());
        }

        // GET: Visits/Details/5
        public ActionResult Details(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visit visit = db.visit.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // GET: Visits/Create
        public ActionResult Create()
        {
            ViewBag.hosipital_id = new SelectList(db.doctor, "id", "name");
            return View();
        }

        // POST: Visits/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "date,hosipital_id,number,price")] visit visit)
        {
            if (ModelState.IsValid)
            {
                db.visit.Add(visit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hosipital_id = new SelectList(db.doctor, "id", "name", visit.doctor_id);
            return View(visit);
        }

        // GET: Visits/Edit/5
        public ActionResult Edit(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visit visit = db.visit.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            ViewBag.hosipital_id = new SelectList(db.doctor, "id", "name", visit.doctor_id);
            return View(visit);
        }

        // POST: Visits/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "date,hosipital_id,number,price")] visit visit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hosipital_id = new SelectList(db.doctor, "id", "name", visit.doctor_id);
            return View(visit);
        }

        // GET: Visits/Delete/5
        public ActionResult Delete(DateTime id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            visit visit = db.visit.Find(id);
            if (visit == null)
            {
                return HttpNotFound();
            }
            return View(visit);
        }

        // POST: Visits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(DateTime id)
        {
            visit visit = db.visit.Find(id);
            db.visit.Remove(visit);
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
