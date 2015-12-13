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
    public class AdministratorsController : Controller
    {
        private DB db = new DB();

        // GET: administrators
        public ActionResult Index()
        {
            return View(db.administrator.ToList());
        }


        public ActionResult Login()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "name,password")] administrator adm)
        {
            if (ModelState.IsValid)
            {
                var admin = db.administrator.FirstOrDefault(o => o.name == adm.name && o.password == adm.password);
                if (admin == null)
                    return View();
                Session["admin"] = admin.name;
                return RedirectToAction("Details", "Administrators");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("admin");
            return RedirectToAction("Login", "Administrators");

        }

        public ActionResult Details()
        {
            if (Session["admin"]==null)
                return RedirectToAction("Login","Administrators");
            string adminname = Session["admin"].ToString();
            var admin = db.administrator.FirstOrDefault(o => o.name == adminname);
            if (admin == null)
                return HttpNotFound();
            return View(admin);
        }

        public ActionResult UserList()
        {
            var userlist = db.user.Where(o => o.is_activated == null);
            return View(userlist.ToList());
        }

        // activate user if exits
        public ActionResult ActivateUser(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            user userinfo = db.user.Find(id);
            userinfo.is_activated = 1;
            db.SaveChanges();

            string admin= Session["admin"].ToString();
            int adminid = db.administrator.FirstOrDefault(o => o.name == admin).id;
            return RedirectToAction("UserList", "Administrators");
        }

        //judge if a user's apointment have over time. if so, decrease its credict
        public ActionResult BlackList()
        {
            DateTime today = DateTime.Now;
            foreach (user u in db.user)
            {
                var appo = db.appointment.Where(a => a.user_id == u.id);
                foreach (appointment app in appo)
                {
                    DateTime appday = (DateTime)app.time;
                    TimeSpan t = appday - today;
                    int days = t.Days;
                    if (days <= 0)
                        u.credict_rank -= 1;

                }
            }
            db.SaveChanges();
            return RedirectToAction("UserList", "Administrators");
        }
        
    }
}
