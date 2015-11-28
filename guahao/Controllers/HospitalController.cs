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
        // GET: Hospital
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HospitalList(string cityname)
        {
            guahaoEntities es = new guahaoEntities();
            var db = new guahaoEntities();
            //var hos = db.hospital.Where(o => o.city.Equals(1)).FirstOrDefault();
            var hos = (from h in es.hospital
                               where h.city==1
                               select h.name).FirstOrDefault();

            ViewBag.name = hos;         
            return View();
        }
    }
}