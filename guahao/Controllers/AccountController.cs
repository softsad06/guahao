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
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (string.IsNullOrWhiteSpace(Request.Form["uname"]) == false &&
            string.IsNullOrWhiteSpace(Request.Form["upwd"]) == false)
            {
                guahaoEntities es = new guahaoEntities();
                var userinfo = from s in es.user
                                   select s;
                string uname = Request.Form["uname"];
                string upwd = Request.Form["upwd"];

                foreach (var user in userinfo)
                {
                    if (user.name == uname && upwd == user.password)
                    {
                        Response.Redirect("~/Home/Index");
                        //return View();
                    }

                }
                Session["user"] = uname;

            }
            return View();
        }

        public ActionResult Signup()
        {
            //ViewBag.Message = "name=" + Request.Form["uname"].ToString();
            if (string.IsNullOrWhiteSpace(Request.Form["uname"]) == false &&
            string.IsNullOrWhiteSpace(Request.Form["upwd"]) == false)
            {
                guahaoEntities es = new guahaoEntities();
                var userinfo = from s in es.user
                                   select s;
                string uname = Request.Form["uname"];
                string upwd = Request.Form["upwd"];
                string upwd2 = Request.Form["upwd2"];
                int newid = userinfo.Count()+1;

                if(upwd==upwd2)
                {
                    foreach (var user in userinfo)
                    {
                        if (user.name == uname)
                        {
                            return View();
                        }

                    }

                    es.user.Add(new user { id=newid,name = uname, password = upwd,
                        real_name ="",social_id="", tel="",email="",is_activated=0,
                    credict_rank=0,picture=""});
                    es.SaveChanges();
                    Response.Redirect("Login");
                }

               

            }
            return View();

        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            Response.Redirect("~/Home/Index");
            return View();

        }

        public ActionResult AddUserInfo()
        {

            if (string.IsNullOrWhiteSpace(Request.Form["urealname"]) == false &&
           string.IsNullOrWhiteSpace(Request.Form["urealid"]) == false)
            {
                guahaoEntities es = new guahaoEntities();
                var userinfo = from s in es.user
                                   select s;

                string uname = Request.Form["urealname"];
                string upwd = Request.Form["urealid"];

                foreach (var user in userinfo)
                {
                    if (user.name == uname)
                    {
                        return View();
                    }

                }

                es.user.Add(new user { name = uname, password = upwd });
                es.SaveChanges();
                Response.Redirect("~/Home/Index");

            }
            return View();
        }
    }

}
