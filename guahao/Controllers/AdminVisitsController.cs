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
    public class AdminVisitsController : Controller
    {
        private DB db = new DB();

        // GET: AdminVisits
        public ActionResult Index()
        {
            string username = Session["admin"].ToString();
            var admin = db.administrator.Where(a => a.name == username).First().hospital_id;
            var per = db.administrator.Where(a => a.name == username).First().permission;
            if(per==1)
            {
                var res = (from hos in db.hospital.AsParallel()
                           where hos.id == admin
                           join dep in db.department.AsParallel() on hos.id equals dep.hospital_id
                           join doc in db.doctor.AsParallel() on dep.id equals doc.department_id
                           join vis in db.visit.AsParallel() on doc.id equals vis.doctor_id
                           select vis);
                return View(res.ToList().Take(50));
            }
            return View(db.visit.ToList());

        }

        // GET: AdminVisits/Details/5
        public ActionResult Details(int? id)
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

        // GET: AdminVisits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminVisits/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,date,doctor_id,number,price")] visit visit)
        {
            if (ModelState.IsValid)
            {
                db.visit.Add(visit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(visit);
        }

        // GET: AdminVisits/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: AdminVisits/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,date,doctor_id,number,price")] visit visit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(visit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(visit);
        }

        // GET: AdminVisits/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: AdminVisits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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
