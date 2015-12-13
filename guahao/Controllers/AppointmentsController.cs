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
            int i = db.appointment.Count() + 1;
            if (i != 1)
                i = db.appointment.Max(o => o.id) + 1;
            return i;
        }

        // GET: Appointments/Create
        public ActionResult Create(int? did)
        {
            if (did == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["user"]==null)
                return RedirectToAction("Login", "Account");
            string uname = Session["user"].ToString();
            user user = db.user.FirstOrDefault(a => a.name == uname);
            // the case that user not login or not activated or has been black
            if (user==null||user.is_activated==null||user.credict_rank<=0)
            {
                return RedirectToAction("Login", "Account");
            }
           
            ViewBag.user_id = user.id;
            ViewBag.user_name = uname;
            ViewBag.doctor_id = did;
            doctor doctor = db.doctor.Find(did);
            ViewBag.doctor_name = doctor.name;
            ViewData["id"] = IdGet() ;
            DateTime dt= Convert.ToDateTime(Session["appointment_date"].ToString());
            ViewBag.time = dt;

            //the case that user had ordered more than 3 doctor at the same time
            int  app = db.appointment.Count(o => o.user_id == user.id&&o.time==dt);
            if (app>3)
                return RedirectToAction("Index", "Home");

            //the case order the same doctor
            var appp = db.appointment.FirstOrDefault(o => o.user_id == user.id && o.time == dt && o.doctor_id == did);
            if (appp!=null)
                return RedirectToAction("Index");
            //the case order the a doctor which in the same depatment
            //var apppp = db.appointment.Include(o => o.doctor.department_id == doctor.department_id).FirstOrDefault(o => o.user_id == user.id && o.time == dt);
            var apppp = from a in db.appointment
                        where a.user_id == user.id && a.time == dt
                        join d in db.doctor on a.doctor_id equals d.id
                        where d.department_id==doctor.department_id
                        select a;
            if (apppp.Count()>=1)
                return RedirectToAction("Index");
            var depart = db.department.FirstOrDefault(d=>d.id==doctor.department_id);
            var hospital = db.hospital.Find(depart.hospital_id);
            ViewBag.hospital_id = hospital.id;
            ViewBag.hospital_name = hospital.name;
            ViewBag.status = 0;
            ViewBag.price = db.visit.FirstOrDefault(o => o.date == dt && o.doctor_id == did).price;
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,hospital_id,user_id,doctor_id,time,status,price")] appointment appointment)
        {
            if (ModelState.IsValid)
            {
                db.appointment.Add(appointment);
                DateTime dt = Convert.ToDateTime(Session["appointment_date"].ToString());
                visit v = db.visit.FirstOrDefault(o => o.date == dt && o.doctor_id == appointment.doctor_id);
                v.number -= 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.doctor_id = new SelectList(db.doctor, "id", "name", appointment.doctor_id);
            ViewBag.hospital_id = new SelectList(db.hospital, "id", "name", appointment.hospital_id);
            ViewBag.user_id = new SelectList(db.user, "id", "name", appointment.user_id);
            return View(appointment);
        }

       
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
            DateTime today = DateTime.Now;
            DateTime appday = (DateTime)appointment.time;
            TimeSpan t = appday - today;
            int days = t.Days;
            //before the visit day
            if (days >= 1)
                return View(appointment);
            else
                return RedirectToAction("Index");
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            appointment app = db.appointment.Find(id);
            var v = db.visit.FirstOrDefault(o => o.date == app.time && o.doctor_id == app.doctor_id);
            v.number += 1;
            db.appointment.Remove(app);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Pay(int? aid)
        {
            Random r = new Random();
            int key1 = r.Next(0, 999999999);
            int key2 = r.Next(0, 999999999);
            ViewBag.payNumber = key1.ToString() + key2.ToString();
            ViewBag.id = aid;

            string uname = Session["user"].ToString();
            var userinfo = db.user.FirstOrDefault(o => o.name == uname);
            var app = db.appointment.Find(aid);
            doctor doc = db.doctor.Find(app.doctor_id);
            department dep = db.department.Find(doc.department_id);
            hospital hos = db.hospital.Find(dep.hospital_id);

            ViewBag.user_name = userinfo.real_name;
            ViewBag.doctor_name = doc.name;
            ViewBag.hospital_name = hos.name;
            ViewBag.time = app.time;
            ViewBag.price = app.price;
            return View();
        }

        [HttpPost]
        public ActionResult Pay2(int id)
        {
            string uname = Session["user"].ToString();
            var appoint = db.appointment.Where(a=>a.id==id).Include(a => a.doctor).Include(a => a.hospital).Include(a => a.user).Where(a => a.user.name == uname);
            foreach (appointment app in appoint)
            {
                app.status = 1;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Print1(int ?id)
        {
            var app = db.appointment.Find(id);
            return View(app);
        }

        public ActionResult Print2(int? id)
        {
            var app = db.appointment.Find(id);
            return View(app);
        }
    }
}
