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
                user u = db.user.FirstOrDefault(x => x.password == userinfo.password && x.name == userinfo.name);
                if (u == null)
                {
                    ViewBag.NextUrl = "~/Account/Login";
                    ViewBag.Message = "登陆出错。请检查用户名或密码是否填写正确";
                    return View("~/Views/Shared/Message.cshtml");
                }
                Session["user"] = userinfo.name;
                //如果session有存储查询期间的信息，将跳转到查询到的医生页面
                if (Session["hospital"] != null && Session["department"] != null && Session["doctor"] != null)
                {
                    return RedirectToAction("DoctorDetail", "Hospital");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.NextUrl = "~/Account/Login";
            ViewBag.Message = "登陆出错。请检查用户名或密码是否填写正确";
            return View("~/Views/Shared/Message.cshtml");
        }


        public ActionResult Signup()
        {
            return View();
        }

        public bool CheckName(string name)
        {
            user u = db.user.FirstOrDefault(x => x.name == name);
            if (u != null)
                return false;
            else
                return true;
        }

        [HttpPost]
        public ActionResult Signup([Bind(Include = "name,password")]user userinfo)
        {
            if (ModelState.IsValid)
            {
                user u = db.user.FirstOrDefault(x => x.name == userinfo.name);
                if (u != null)
                {
                    ViewBag.NextUrl = "~/Account/Login";
                    ViewBag.Message = "注册出错。请检查用户名或密码是否填写正确";
                    return View("~/Views/Shared/Message.cshtml");
                }
                userinfo.id = db.user.Count() + 1;
                userinfo.password = userinfo.Md5Helper(userinfo.password);
                userinfo.credict_rank = 5;
                db.user.Add(userinfo);
                db.SaveChanges();
                ViewBag.NextUrl = "~/Account/Login";
                ViewBag.Message = "恭喜您，账号" + userinfo.name + "注册成功。";
                return View("~/Views/Shared/Message.cshtml");
            }
            ViewBag.NextUrl = "~/Account/Login";
            ViewBag.Message = "注册出错。请检查用户名或密码是否填写正确";
            return View("~/Views/Shared/Message.cshtml");
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            //ViewBag.NextUrl();
            ViewBag.Message = "登出成功。";
            ViewBag.NextUrl = "~";
            return View("~/Views/Shared/Message.cshtml");
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
            ViewBag.NextUrl = "~/Account/UserInfo";
            ViewBag.Message = "信息填写完成。管理员将根据您输入的个人身份数据，对此账号进行审核。审核通过后，您可以使用预约功能。";
            return View("~/Views/Shared/Message.cshtml");
        }
    }

}
