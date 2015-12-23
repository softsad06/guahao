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
        public ActionResult Create(int? did,string date)
        {
            if (did == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Session["user"]==null)
            {
                ViewBag.NextUrl = "~/Account/Login";
                ViewBag.Message = "请先注册或登录。";
                return View("~/Views/Shared/Message.cshtml");
            }
            string uname = Session["user"].ToString();
            user user = db.user.FirstOrDefault(a => a.name == uname);
            // the case that user not login or not activated or has been black
            if (user==null||user.is_activated==null||user.credict_rank<=0)
            {
                ViewBag.NextUrl = "~/Account/UserInfo";
                ViewBag.Message = "您的账号还未实名认证，或者信用等级过低，无法进行预约操作。";
                return View("~/Views/Shared/Message.cshtml");
            }
           
            ViewBag.user_id = user.id;
            ViewBag.user_name = uname;
            ViewBag.doctor_id = did;
            doctor doctor = db.doctor.Find(did);
            ViewBag.doctor_name = doctor.name;
            ViewData["id"] = IdGet() ;
            if (date == null || date.Length <= 0)
            {
                if (Session["appointment_date"] != null)
                {
                    date = Session["appointment_date"].ToString();
                }
            }
            DateTime dt = Convert.ToDateTime(date);
            ViewBag.time = dt;
            //the case that user had ordered more than 3 doctor at the same time
            int  app = db.appointment.Count(o => o.user_id == user.id&&o.time==dt);
            if (app>3)
            {
                ViewBag.Message = "同一时间段内不能预约超过三个医生，请稍后再试。";
                return View("~/Views/Shared/Message.cshtml");
            }
            //the case order the same doctor
            var appp = db.appointment.FirstOrDefault(o => o.user_id == user.id && o.time == dt && o.doctor_id == did);
            if (appp != null)
            {
                ViewBag.Message = "不能重复预约。";
                return View("~/Views/Shared/Message.cshtml");
            }
            //the case order the a doctor which in the same depatment
            //var apppp = db.appointment.Include(o => o.doctor.department_id == doctor.department_id).FirstOrDefault(o => o.user_id == user.id && o.time == dt);
            var apppp = from a in db.appointment
                        where a.user_id == user.id && a.time == dt
                        join d in db.doctor on a.doctor_id equals d.id
                        where d.department_id==doctor.department_id
                        select a;
            if (apppp.Count()>=1)
            {
                ViewBag.Message = "不能在同一时间段内预约同一科室的医生。";
                return View("~/Views/Shared/Message.cshtml");
            }
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
                ViewBag.NextUrl = "~/Appointments/Index";
                ViewBag.Message = "预约单提交成功！请查看您的预约信息。";
                return View("~/Views/Shared/Message.cshtml");
            }

            ViewBag.doctor_id = new SelectList(db.doctor, "id", "name", appointment.doctor_id);
            ViewBag.hospital_id = new SelectList(db.hospital, "id", "name", appointment.hospital_id);
            ViewBag.user_id = new SelectList(db.user, "id", "name", appointment.user_id);
            ViewBag.Message = "预约失败。请查看您的预约信息是否正确。";
            return View("~/Views/Shared/Message.cshtml");
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

            ViewBag.NextUrl = "~/Appointments/Index";
            ViewBag.Message = "支付成功！";
            return View("~/Views/Shared/Message.cshtml");
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
