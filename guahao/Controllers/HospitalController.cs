using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using guahao.Models;
using System.Data.Entity;

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
                var city = db.city.FirstOrDefault(o => o.name == cityname);
                if (city != null)
                {
                    string city_abv = db.city.FirstOrDefault(o => o.name == cityname).abr_name;
                    var hos = db.hospital.Where(o => o.city == city_abv);
                    return View(hos.ToList());
                }
                else
                    return View(db.hospital.ToList());
                    
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


        public ActionResult DoctorList(int ?id, string date)
        {
            if (ModelState.IsValid)
            {
                if (date != null)
                {
                    DateTime dt = Convert.ToDateTime(date);
                    Session["appointment_date"] = date;
                    var doc = from d in db.doctor
                              where d.department_id == id
                              join v in db.visit on d.id equals v.doctor_id
                              where v.date == dt && v.number > 0
                              select d;
                    return View(doc.ToList());
                }
                else
                {
                    var doc = db.doctor.Where(d => d.department_id == id).ToList();
                    return View(doc);
                }

            }
            return View();
        }
        

        public JsonResult LoadDoctor()
        {
             var doc = db.doctor.Where(o => o.department_id == 1);
            return Json(doc);

        }

        public ActionResult DoctorDetail(int? id)
        {
            //Session["user"] = "lykeven";
            var docDetail = db.doctor.Find(id);
            if (docDetail == null)
            {
                return HttpNotFound();
            }
            return View(docDetail);
        }


    }
}