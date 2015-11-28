using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using guahao.Models;

namespace guahao.Controllers
{
    public class AccountController : Controller
    {
        private DB db = new DB();

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(user userinfo)
        {
            if (ModelState.IsValid)
            {
                userinfo.password = userinfo.Md5Helper(userinfo.password);
                user u = db.user.First(x => x.password == userinfo.password);
                Session["user"] = userinfo.name;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(user userinfo,string password2)
        {
            if (ModelState.IsValid&&userinfo.password==password2 )
            {
                user u = db.user.FirstOrDefault(x => x.password == password2);
                if (u != null)
                    return View();
                userinfo.id = db.user.Count() + 1;
                userinfo.password = userinfo.Md5Helper(userinfo.password);
                db.user.Add(userinfo);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            return RedirectToAction("Index", "Home");

        }

        public ActionResult UserInfo()
        {
            
            if (Session["user"]!=null)
            {
                string uname = Session["user"].ToString();
                user u = db.user.FirstOrDefault(x => x.name == uname);
                return View("UserInfo", u);

            }
            return View();
        }
        public ActionResult AddUserInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserInfo([Bind(Include ="picture,email,tel,social_id,real_name")]user userinfo)
        {
            if (ModelState.IsValid)
            {

                string uname = Session["user"].ToString();
                user userchange = db.user.First(x => x.name == uname);
                userchange.email = userinfo.email != null ? userinfo.email : userchange.email;
                userchange.tel = userinfo.tel != null ? userinfo.tel : userchange.tel;
                userchange.social_id = userinfo.social_id != null ? userinfo.social_id : userchange.social_id;
                userchange.real_name = userinfo.real_name != null ? userinfo.real_name : userchange.real_name;
                userchange.picture = userinfo.picture != null ? userinfo.picture : userchange.picture;
                db.SaveChanges();
                return RedirectToAction("UserInfo", "Account");
            }
            return View();
        }
    }

}
