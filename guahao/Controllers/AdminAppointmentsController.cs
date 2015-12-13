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
    public class AdminAppointmentsController : Controller
    {
        private DB db = new DB();

        // GET: AdminAppointments
        public ActionResult Index()
        {
            var appointment = db.appointment.Include(a => a.doctor).Include(a => a.hospital).Include(a => a.user);
            return View(appointment.ToList());
        }

        // GET: AdminAppointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: AdminAppointments/Create
        public ActionResult Create()
        {
            ViewBag.doctor_id = new SelectList(db.doctor, "id", "name");
            ViewBag.hospital_id = new SelectList(db.hospital, "id", "name");
            ViewBag.user_id = new SelectList(db.user, "id", "name");
            return View();
        }

        // POST: AdminAppointments/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,hospital_id,user_id,doctor_id,time,status,price")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.appointment.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doctor_id = new SelectList(db.doctor, "id", "name", appointment.doctor_id);
            ViewBag.hospital_id = new SelectList(db.hospital, "id", "name", appointment.hospital_id);
            ViewBag.user_id = new SelectList(db.user, "id", "name", appointment.user_id);
            return View(appointment);
        }

        // GET: AdminAppointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.doctor_id = new SelectList(db.doctor, "id", "name", appointment.doctor_id);
            ViewBag.hospital_id = new SelectList(db.hospital, "id", "name", appointment.hospital_id);
            ViewBag.user_id = new SelectList(db.user, "id", "name", appointment.user_id);
            return View(appointment);
        }

        // POST: AdminAppointments/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,hospital_id,user_id,doctor_id,time,status,price")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.doctor_id = new SelectList(db.doctor, "id", "name", appointment.doctor_id);
            ViewBag.hospital_id = new SelectList(db.hospital, "id", "name", appointment.hospital_id);
            ViewBag.user_id = new SelectList(db.user, "id", "name", appointment.user_id);
            return View(appointment);
        }

        // GET: AdminAppointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            appointment appointment = db.appointment.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: AdminAppointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appointment appointment = db.appointment.Find(id);
            db.appointment.Remove(appointment);
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
