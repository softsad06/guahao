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
    public class AppointmentsController : Controller
    {
        private DB db = new DB();

        // GET: Appointments
        public ActionResult Index()
        {
            string uname = Session["user"].ToString();
            var appointment = db.appointment.Include(a => a.doctor).Include(a => a.hospital).Include(a => a.user).Where(a => a.user.name == uname);
            return View(appointment.ToList());
        }

        // GET: Appointments/Details/5
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
        public int IdGet()
        {
            int i = db.appointment.Max(a => a.id) + 1;
            return i;
        }

        // GET: Appointments/Create
        public ActionResult Create(int? did)
        {
            if (did == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewData["id"] = IdGet() ;
            ViewBag.doctor_id = did;
            doctor doctor = db.doctor.Find(did);
            ViewBag.doctor_name = doctor.name;
            
            string uname=Session["user"].ToString();
            ViewBag.user_name = uname;
            user user= db.user.Where( a =>a.name== uname).First();
            ViewBag.user_id =user.id; 

            ViewBag.time = DateTime.Now;
            department depart = db.department.Find(doctor.id);
            hospital hospital = db.hospital.Find(depart.hospital_id);
            ViewBag.hospital_id = hospital.id;
            ViewBag.hospital_name = hospital.name;
            ViewBag.status = 0;
            ViewBag.price = 888;
            return View();
        }

        // POST: Appointments/Create
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

        // GET: Appointments/Edit/5
       /* public ActionResult Edit(int? id)
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

        // POST: Appointments/Edit/5
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
        }*/

        // GET: Appointments/Delete/5
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

        // POST: Appointments/Delete/5
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
