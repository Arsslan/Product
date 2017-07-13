using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using DAL;
namespace Assignment5.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            string user = Session["user"].ToString();
            if (user != null || user != "")
            {
                Dal obj = new Dal();

                user us = obj.getuser(user);
                ViewBag.user = us;
                return View();
            }
            else
            {
                return Redirect("~/Product/Login");
            }
        }
        [HttpPost]
        public ActionResult UpdateProfile(user user1)
        {

            if (user1.Login == null || user1.Password == null || user1.Name == null)
            {
                ViewBag.missing = "Must Fill Mandatory Feilds";
                return Redirect("~/Product/ProductView");

            }
            else
            {
                string admin = Session["user"].ToString();
                Dal obj = new Dal();
                var uname = "6c26fab6-21eb-4483-9242-7a18c2b52104.jpg";
                if (Request.Files["picurl"] != null)
                {
                    var file = Request.Files["picurl"];
                    if (file.FileName != "")
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        uname = Guid.NewGuid().ToString() + ext;
                        var rootpath = Server.MapPath(Url.Content("~/Content/Images"));
                        var savpath = System.IO.Path.Combine(rootpath, uname);
                        file.SaveAs(savpath);
                    }
                }
                user1.PicUrl = uname;
                if (obj.UpdateUser(user1, admin))
                {
                    ViewBag.missing = "User Updated Succcessfully";
                    return Redirect("~/Product/ProductView");
                }
                else
                {

                    ViewBag.error = "Udation Failed";

                    return View();
                }
            }
        }
	}
}