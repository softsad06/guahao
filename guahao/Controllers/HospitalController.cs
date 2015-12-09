using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using guahao.Models;

namespace guahao.Controllers
{
    public class HospitalController : Controller
    {
        private DB db = new DB();
        // GET: Hospital
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HospitalList(string cityname)
        {
            if (ModelState.IsValid)
            {
                string city_abv = db.city.FirstOrDefault(o=>o.name==cityname).abr_name;
                var hos = db.hospital.Where(o => o.city == city_abv);
                return View(hos.ToList());
            }
            return View();
        }

        public ActionResult HospitalDetail(int? id)
        {
            var hosDetail = db.hospital.Find(id);
            if (hosDetail == null)
            {
                return HttpNotFound();
            }
            return View(hosDetail);
        }

        public ActionResult DepartmentList(int ?id)
        { 
            if (ModelState.IsValid)
            {
                var dep = db.department.Where(o => o.hospital_id==id);
                return View(dep.ToList());
            }
            return View();
        }

        public ActionResult DepartmentDetail(int? id)
        {
            var depDetail = db.department.Find(id);
            if (depDetail == null)
            {
                return HttpNotFound();
            }
            return View(depDetail);
        }

        public ActionResult DoctorList(int ?id)
        {
            if (ModelState.IsValid)
            {
                var doc = db.doctor.Where(o => o.department_id == id);
                return View(doc.ToList());
            }
            return View();
        }

        public ActionResult DoctorDetail(int? id)
        {
            var docDetail = db.doctor.Find(id);
            if (docDetail == null)
            {
                return HttpNotFound();
            }
            return View(docDetail);
        }

    }
}