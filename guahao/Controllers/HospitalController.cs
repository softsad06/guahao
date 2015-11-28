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
                var hos = db.hospital.Where(o => o.city == 1);
                return View(hos.ToList());
            }
            return View();
        }

        public ActionResult Detail(int? id)
        {
            var hosDetail = db.hospital.Find(id);
            if (hosDetail == null)
            {
                return HttpNotFound();
            }
            return View(hosDetail);
        }
    }
}