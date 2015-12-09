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
    public class administratorsController : Controller
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
        public ActionResult Login([Bind(Include = "name,password,hospital_id")] administrator adm)
        {
            if (ModelState.IsValid)
            {
                var admin = db.administrator.FirstOrDefault(o => o.name == adm.name && o.password == adm.password && o.hospital_id == adm.hospital_id);
                if (admin == null)
                    return View();
                Session["user"] = admin.name;
                return RedirectToAction("Details", "Administrators");
            }
            return View();
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            administrator administrator = db.administrator.Find(id);
            if (administrator == null)
            {
                return HttpNotFound();
            }
            return View(administrator);
        }

    }
}
