using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;

namespace Assignment5.Controllers
{
    public class CommentController : Controller
    {
        [HttpPost]
        public JsonResult Addcomment(string Productid, string Userid, string comment)
        {
            Object data = null;
            var f = false;
            String url = "";
            if (comment != "" && comment != null)
            {
                Dal obj = new Dal();
                if (obj.Addcommentt(Convert.ToInt32(Productid), Convert.ToInt32(Userid), comment.ToString()))
                {
                    url = Url.Content("~/Product/ProductView");
                    f = true;
                }
                else
                {
                    f = false;
                }
                data = new
                {
                    flag = f,
                    urli = url
                };
            }

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getcomments(string productid)
        {
            var f = true;
            int pid = Convert.ToInt32(productid);
            Dal obj=new Dal();
            var comments = obj.GetAllcomments(pid);
           var data = new
            {
               cmt = comments,
                flag=f
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
       
	}
}