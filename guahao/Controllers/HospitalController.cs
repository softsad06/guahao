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

        public ActionResult SearchHospitalByName(string hname)
        {
            int number = 20;
            int page = 1;
            if (hname.Length <= 0)
            {
                var res = db.hospital.OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList();
                Session.Remove("city");
                return View("~/Views/Hospital/HospitalList.cshtml", res);
            }
            else
            {
                var res = db.hospital.Where(o => o.name.IndexOf(hname) != -1).OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList();
                Session["city"] = "\"" + hname + "\"";
                return View("~/Views/Hospital/HospitalList.cshtml", res);
            }
        }

        public ActionResult HospitalList(string cityname)
        {
            int number = 20;
            int page = 1;
            if (ModelState.IsValid)
            {
                var city = db.city.FirstOrDefault(o => o.name == cityname);
                if (city != null)
                {
                    string city_abv = db.city.FirstOrDefault(o => o.name == cityname).abr_name;
                    var hos = db.hospital.Where(o => o.city == city_abv).OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList();
                    Session["city"] = cityname;
                    return View(hos);
                }
                else
                {
                    Session.Remove("city");
                    return View(db.hospital.OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList());
                }
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
            Session["hospital"]=hosDetail.name;
            return View(hosDetail);
        }

        public ActionResult DepartmentList(int ?id)
        {
            int number = 20;
            int page = 1;
            if (ModelState.IsValid)
            {
                var dep = db.department.Where(o => o.hospital_id == id).OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList();
                return View(dep);
            }else{
                return View();
            }
        }

        public ActionResult DepartmentDetail(int? id)
        {
            var depDetail = db.department.Find(id);
            if (depDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                Session["department"] = depDetail.name;
                return View(depDetail);
            }
        }


        public ActionResult DoctorList(int ?id, string date)
        {
            int number = 20;
            int page = 1;
            if (ModelState.IsValid)
            {
                if (id == null) Session["department"] = "";
                else Session["department"] = db.department.Where(d => d.id == id).First().name;
                if (date != null && date.Length>0)
                {
                    DateTime dt = Convert.ToDateTime(date);
                    Session["appointment_date"] = date;
                    var doc = from d in db.doctor
                              where d.department_id == id
                              join v in db.visit on d.id equals v.doctor_id
                              where v.date == dt && v.number > 0
                              select d;
                    return View(doc.OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList());
                }
                else
                {
                    Session["appointment_date"] = "";
                    var doc = db.doctor.Where(d => d.department_id == id).OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList();
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
            else
            {
                Session["dotor"] = docDetail.name;
                return View(docDetail);
            }
        }


    }
}