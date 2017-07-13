using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entities;
using DAL;
namespace Assignment5.Controllers
{
    public class FavoriteController : Controller
    {

        public JsonResult Addtofavorit(string Productid, string Userid)
        {
            Object data = null;
            string url = "";
            int pid = Convert.ToInt32(Productid);
            int uid = Convert.ToInt32(Userid);
            Dal obj = new Dal();
            if(obj.Addtofavorit(pid,uid))
            {
                url=Url.Content("~/Product/ProductView");
            }
            data = new
            {
                urli = url
            };
           return Json(data,JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewFavourite()
        {
            Dal obj = new Dal();
           int uid= Convert.ToInt32(Session["UserId"]);
           List<product> flist = obj.ViewFavorite(uid);
           ViewBag.favoritelist = flist;
            return View();
        }
	}
}