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
            Session["department"] = "";
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
            Session["department"] = "";
            if (ModelState.IsValid)
            {
                //如果直接进入此页面，根据session找对应的医院
                if (id == null)
                {
                    if (Session["hospital"] != null)
                    {
                        var hosp = db.hospital.Where(h => h.name == Session["hospital"]).FirstOrDefault();
                        if (hosp == null)
                        {
                            return HttpNotFound();
                        }
                        else
                        {
                            id = hosp.id;
                        }
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }
                var dep = db.department.Where(o => o.hospital_id == id).OrderBy(o => o.id).Skip((page - 1) * number).Take(number).ToList();
                return View(dep);
            }else{
                return View();
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
            Session["department"] = "";
            return View();
        }
        

        public JsonResult LoadDoctor()
        {
             var doc = db.doctor.Where(o => o.department_id == 1);
            return Json(doc);
        }

        public ActionResult DoctorDetail(int? id)
        {
            if (id == null)
            {
                //根据Session原有记录打开相应的医生页面
                if(Session["hospital"]!=null && Session["department"]!=null && Session["doctor"]!=null)
                {
                    string hname = Session["hospital"].ToString();
                    string dname = Session["department"].ToString();
                    string docname = Session["doctor"].ToString();
                    var deps1 = db.hospital.Where(h => h.name == hname).FirstOrDefault();
                    if (deps1 != null)
                    {
                        var deps = deps1.department;
                        var docs = deps.Where(d => d.name == dname).First().doctor;
                        var docDetail1 = docs.Where(doc => doc.name == docname).First();
                        return View(docDetail1);
                    }
                }
            }
            var docDetail = db.doctor.Find(id);
            if (docDetail == null)
            {
                return HttpNotFound();
            }
            else
            {
                Session["doctor"] = docDetail.name;
                return View(docDetail);
            }
        }

        public ActionResult getVisitsByDate(int did,string date)
        {
            DateTime datetime = new DateTime(1999,1,1);
            try{
                 datetime=DateTime.Parse(date);
            }catch(Exception){}

            try {
                var sb=datetime.Date;
                var res = db.visit.Where(v => v.doctor_id == did && v.date == datetime).ToList();
                string result = "";
                foreach (var d in res)
                {
                    result += d.date.ToString() + ",";
                    result += d.number.ToString() + ",";
                    result += d.price.ToString() + ",";
                    result += d.id.ToString() + ",";
                }
                if (result.Length >= 1) result = result.Substring(0, result.Length - 1);
                return Content(result);
            }
            catch (Exception) { return View(); }

        }

    }
}