using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using System.Security.Cryptography;
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

            return View();
        }
        [HttpPost]
        public ActionResult Login(user userinfo)
        {
            guahaoEntities es = new guahaoEntities();
            var userindb = from s in es.user
                           select s;
            userinfo.password = Md5Helper(userinfo.password);
            foreach (var user in userindb)
            {
                if (user.name == userinfo.name&&user.password==userinfo.password)
                {
                    Response.Redirect("~/Home/Index");
                }

            }
            Session["user"] = userinfo.name;
            return View();
        }


        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup([Bind(Include ="name,password")]user userinfo,string password2)
        {
            if (userinfo.password==password2)
            {
                guahaoEntities es = new guahaoEntities();
                var userindb = from s in es.user
                                   select s;
                int newid = userindb.Count()+1;
                foreach (var user in userindb)
                {
                    if (user.name == userinfo.name)
                        return View();
                }
                userinfo.password = Md5Helper(userinfo.password);
                es.user.Add(new user
                {
                    id = newid,
                    name = userinfo.name,
                    password = userinfo.password,
                    real_name = "",
                    social_id = "",
                    tel = "",
                    email = "",
                    is_activated = 0,
                    credict_rank = 0,
                    picture = ""
                });
                es.SaveChanges();
                Response.Redirect("Login");

            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            Response.Redirect("~/Home/Index");
            return View();

        }

        public ActionResult UserInfo()
        {
            
            if (Session["user"]!=null)
            {
                string uname = Session["user"].ToString();
                guahaoEntities es = new guahaoEntities();
                var userinfo = from s in es.user
                               where s.name==uname
                               select s;
                return View("UserInfo", userinfo.FirstOrDefault());

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
            
            string uname = Session["user"].ToString();
            guahaoEntities es = new guahaoEntities();
            var userdb = from s in es.user
                           where s.name == uname
                           select s;
            user userchange = userdb.FirstOrDefault();
            userchange.email = userinfo.email != null ? userinfo.email : userchange.email;
            userchange.tel = userinfo.tel != null ? userinfo.tel : userchange.tel;
            userchange.social_id = userinfo.social_id != null ? userinfo.social_id : userchange.social_id;
            userchange.real_name = userinfo.real_name != null ? userinfo.real_name : userchange.real_name;
            userchange.picture = userinfo.picture != null ? userinfo.picture : userchange.picture;
            es.SaveChanges();
            Response.Redirect("~/Home/Index");
            return View();
        }

        public string Md5Helper(string password)
        {

            byte[] result = Encoding.Default.GetBytes(password);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            password = BitConverter.ToString(output).Replace("-", "");
            return password;
        }
    }

}
