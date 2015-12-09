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
                user u = db.user.FirstOrDefault(x => x.password == userinfo.password&&x.name==userinfo.name);
                if (u==null)
                    return View();
                Session["user"] = userinfo.name;
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        public ActionResult Signup()
        {
            return View();
        }

        public string CheckName(string name)
        {
            user u = db.user.FirstOrDefault(x => x.name == name);
            if (u != null)
                return "该用户名已存在";
            else
                return "该用户名可用";
        }

        [HttpPost]
        public ActionResult Signup([Bind(Include = "name,password")]user userinfo)
        {
            if (ModelState.IsValid)
            {
                user u = db.user.FirstOrDefault(x => x.name == userinfo.name);
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
                var u = db.user.FirstOrDefault(x => x.name == uname);
                return View("UserInfo", u);

            }
            return View();
        }
        public ActionResult AddUserInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddUserInfo([Bind(Include ="email,tel,social_id,real_name")]user userinfo)
        {
            string uname = Session["user"].ToString();
            user userchange = db.user.FirstOrDefault(x => x.name == uname);
            userchange.email = userinfo.email != null ? userinfo.email : userchange.email;
            userchange.tel = userinfo.tel != null ? userinfo.tel : userchange.tel;
            userchange.social_id = userinfo.social_id != null ? userinfo.social_id : userchange.social_id;
            userchange.real_name = userinfo.real_name != null ? userinfo.real_name : userchange.real_name;
            db.SaveChanges();
            return RedirectToAction("UserInfo", "Account");
        }
    }

}
