using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Entities;
using DAL;
using System.Net;
using System.Net.Mail;
namespace Assignment5.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        [HttpGet]
        public ActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        public ActionResult Login(user user1)
        {
            Dal obj = new Dal();
            if (user1.Login == null || user1.Password == null)
            {
                ViewBag.error = "Must Fill Mandatory feilds";
                return View();
            }

            else if (obj.IsValid(user1))
            {
                if (obj.IsAdmin(user1))
                {
                    Session["user"] = null;
                    Session["admin"] = user1.Login;
                    int id = obj.GetUserId(user1);
                    Session["adminid"] = id;
                    return Redirect(Url.Content("~/Product/ProductView"));
                }
                else
                {
                    Session["UserId"] = obj.GetUserId(user1);
                    Session["admin"] = null;
                    Session["user"] = user1.Login;
                    return Redirect(Url.Content("~/Product/ProductView"));
                }

            }
            else
            {
                return View();
            }


        }
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(user user1)
        {

            if (user1.Login == null || user1.Password == null || user1.Name == null)
            {
                ViewBag.missing = "Must Fill Mandatory Feilds";
                return View();

            }
            else
            {
                var uname = "6c26fab6-21eb-4483-9242-7a18c2b52104.jpg";
                if(Request.Files["picurl"]!=null)
                {
                    var file = Request.Files["picurl"];
                    if(file.FileName!="")
                    {
                        var ext = System.IO.Path.GetExtension(file.FileName);
                        uname = Guid.NewGuid().ToString() + ext;
                        var rootpath=Server.MapPath(Url.Content("~/Content/Images"));
                        var savpath = System.IO.Path.Combine(rootpath, uname);
                        file.SaveAs(savpath);
                    }
                }
                user1.PicUrl = uname;
                Dal obj = new Dal();               
                if (obj.SignUp(user1))
                {
                    return Redirect(Url.Content("~/Product/ProductView"));
                }
                else
                {
                    ViewBag.error = "Login or User Already Exist";
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Addproduct()
        {
            if (Session["admin"] != null)
            {
                Dal obj = new Dal();
                List<type> list1 = new List<type>();
                list1 = obj.GetList();
                SelectList list = new SelectList(list1, "TypeId", "TypeName");
                ViewBag.listoftypes = list;


                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Product/Login"));
            }
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Addproduct(product obj)
        {

            int adminid = Convert.ToInt32(Session["adminid"]);
            Dal obj1 = new Dal();
            var uname = "6c26fab6-21eb-4483-9242-7a18c2b52104.jpg";
            if (Request.Files["picture"] != null)
            {
                var file = Request.Files["picture"];
                if (file.FileName != "")
                {
                    var ext = System.IO.Path.GetExtension(file.FileName);
                    uname = Guid.NewGuid().ToString() + ext;
                    var rootpath = Server.MapPath(Url.Content("~/Content/Images"));
                    var savpath = System.IO.Path.Combine(rootpath, uname);
                    file.SaveAs(savpath);
                }
            }
            obj.picture = uname;
            obj1.AddProducttobatabse(obj, adminid);
            return Redirect(Url.Content("~/Product/ProductView"));
        }
       
        [HttpGet]
        public ActionResult ProductView()
        {

            Dal obj = new Dal();
            List<product> ProductList;
            if(Session["admin"]!=null)
            {
                int id = Convert.ToInt32(Session["adminid"]);
                ProductList = obj.Loadforadmin(id);
            }
            else if (Session["user"] != null)
            {
                List<comment> cmt = obj.Getcomments();
                List<rate> ratings = obj.GetAllRatings();
                ViewBag.rating = ratings;
                ViewBag.comment = cmt;
                ProductList = obj.Load();
            }
            else
            {
                ProductList = obj.Load();               
            }
            ViewBag.productlistindatabase = ProductList;      
            return View();
        }
         [HttpPost]
        public ActionResult ProductView1(int id)
        {
            int pid = id;
           Dal obj = new Dal();
           Session["SelectedProductid"] = pid;
           if (Request["delete"] != null)
           {
               int admin = (int)Session["adminid"];
               if (obj.DeleteProduct(pid, admin))
               {
                   return Redirect(Url.Content("~/Product/ProductView"));
               }
               else
               {
                   ViewBag.error = "This Admin Have no Permission To Modify This Item";
                   return Redirect(Url.Content("~/Product/ProductView"));
               }
           }
           else if(Request["edit"]!=null)
           {
               int admin = (int)Session["adminid"];
               return Redirect(Url.Content("~/Product/EditProduct"));
               
           }
           else if (Request["email"] != null)
           {
               return Redirect(Url.Content("~/Product/Email"));
           }
           else
           {
               return Redirect(Url.Content("~/Product/ProductView"));
           }
          
        }
       

        public ActionResult Logout()
        {
            Session["admin"] = null;
            Session["user"] = null;
            return Redirect("Login");

        }
        [HttpGet]
        public ActionResult UpdateProfile()
        {
            Dal obj = new Dal();
            string adminuser = Session["admin"].ToString();
            user us = obj.getuser(adminuser);
            ViewBag.user = us;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateProfile(user user1)
        {

            if (user1.Login == null || user1.Password == null || user1.Name == null)
            {
                ViewBag.missing = "Must Fill Mandatory Feilds";
                return Redirect(Url.Content("~/Product/ProductView"));

            }
            else
            {
                string admin = Session["admin"].ToString();
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
               
                if (obj.UpdateAdmin(user1, admin))
                {                   
                    ViewBag.missing = "User Updated Succcessfully";
                    return Redirect(Url.Content("~/Product/ProductView"));
                }
                else
                {
                    ViewBag.error = "Udation Failed";
                    return View();
                }
            }
        }      
        [HttpGet]
        public ActionResult EditProduct()
        {
           if (Session["admin"] != null)
            {
                Dal obj = new Dal();
                List<type> list1 = new List<type>();
                list1 = obj.GetList();
                SelectList list = new SelectList(list1, "TypeId", "TypeName");
                ViewBag.listoftypes = list;
                int id = (int)Session["SelectedProductid"];
               product p= obj.GetProductById(id);
               ViewBag.producut = null;
               ViewBag.producut = p;
                return View();
            }
            else
            {
                return Redirect(Url.Content("~/Product/Login"));
            }
        }
       
        [HttpPost]
        public ActionResult EditProduct(product obj)
        {
            
            int id = (int)Session["SelectedProductid"];

            int adminid = Convert.ToInt32(Session["adminid"]);
            var uname = "6c26fab6-21eb-4483-9242-7a18c2b52104.jpg";
            if (Request.Files["picture"] != null)
            {
                var file = Request.Files["picture"];
                if (file.FileName != "")
                {
                    var ext = System.IO.Path.GetExtension(file.FileName);
                    uname = Guid.NewGuid().ToString() + ext;
                    var rootpath = Server.MapPath(Url.Content("~/Content/Images"));
                    var savpath = System.IO.Path.Combine(rootpath, uname);
                    file.SaveAs(savpath);
                }
            }
            obj.picture = uname;
            Dal obj1 = new Dal();
            obj1.UpdateProduct(obj, id,adminid);
            return Redirect(Url.Content("~/Product/ProductView"));
                }
        [HttpPost]
        public ActionResult Email(string txtEmail ,string id)
        {
            bool f=false;
            string url="";
            Object data = null;
            Dal obj = new Dal();
            int idd = Convert.ToInt32(id);
            product p = obj.GetProductById(idd);
            string name = p.Name.ToString();
            double price = (double)p.Price;
            string description = p.Description.ToString();
            string type = p.temp.ToString();
            if (email_send(txtEmail, name, price, description,type))
            {
               
                    f= true;
                    url=Url.Content("~/Product/ProductView");
                
            }
            else
            {
                url = Url.Content("~/Product/ProductView");
                f = false;
            }
            data = new
               {
                   flag = f,
                   urli = url
               };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public bool email_send(string to,string name,double price,string description,string type)
        {
            if (to != "")
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("email");
                mail.To.Add(to);
                mail.Subject = "Product";
                mail.Body = ("The Name of Product is  " + name + " Price is  " + price + " Type of Product is " + type + " And Description is " + description);



                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("email", "password");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        public ActionResult SearchResults()
        {
            return Redirect(Url.Content("~/Product/ProductView"));
        }
        [HttpPost]
        public ActionResult SearchResults(string txtsearch)
        {
           if(Request["search"]!=null)
           {
               Dal obj = new Dal();
               List<product> psearched = obj.SearchResult(txtsearch);
              ViewBag.searched = psearched;
              return View();               
           }
           else
           {
               return Redirect(Url.Content("~/Product/ProductView"));
           }
        }
        public JsonResult RateProduct(string Productid, string ObtainedPoints, string TotalPoints)
        {
            Object data = null;
            bool rate =false;
            Dal obj = new Dal();
            int pid = Convert.ToInt32(Productid);
            int op = Convert.ToInt32(ObtainedPoints);
            int tp = Convert.ToInt32(TotalPoints);
           if( obj.RateProduct(pid, op, tp))
           {
               data = true;
           }
            
            data = new
            {
                flag = rate
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
       
    }
   
}