using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class UserController : Controller
    {
        private _SalesMatrixDB db = new _SalesMatrixDB();
        private string Base_URL = "http://localhost:1848/api/";
        //private string Base_URL = "192.168.0.113:8003/api/";

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Create(User user)
        {
            if (Session["ImageNumber"] != null)
            {
                user.Picture = Session["ImageNumber"].ToString();
            }
            try
            {
                user.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("user", user).Result;
                if (response.IsSuccessStatusCode)
                {
                    Session["ImageNumber"] = null;
                    return Json("User created", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (Session["ImageNumber"] != null)
                    {
                        string fullPath = Request.MapPath("~/Uploads/" + Session["ImageNumber"].ToString());
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    Session["ImageNumber"] = null;
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
                
            }
            catch
            {
                Session["ImageNumber"] = null;
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = new User();
            try
            {
                user.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("user/" + id).Result;
                response.EnsureSuccessStatusCode();
                user = response.Content.ReadAsAsync<User>().Result;
                return View(user);
            }
            catch
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {            
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("user", user).Result;
                response.EnsureSuccessStatusCode();
                return Json("User Updated", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult ResetPassword(int? id)
        {
            if (id == null)
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            User user = new User();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("user/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    user = response.Content.ReadAsAsync<User>().Result;
                    user.Password = EncodePassword("00000");
                }
                if (user != null)
                {
                    try
                    {
                        HttpResponseMessage response2 = client.PutAsJsonAsync("user", user).Result;
                        return Json("Password Reseted Successfully", JsonRequestBehavior.AllowGet);
                    }
                    catch
                    {
                        return Json("Failed", JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
            return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.DeleteAsync("user/" + id).Result;
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            List<User> userList = new List<User>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("users").Result;

                response.EnsureSuccessStatusCode();
                userList = response.Content.ReadAsAsync<List<User>>().Result;
                return Json(userList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllActiveUser()
        {
            List<User> userList = new List<User>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("activeUsers").Result;

                response.EnsureSuccessStatusCode();
                userList = response.Content.ReadAsAsync<List<User>>().Result;
                return Json(userList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllInactiveUser()  
        {
            List<User> userList = new List<User>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("inactiveUsers").Result;

                response.EnsureSuccessStatusCode();
                userList = response.Content.ReadAsAsync<List<User>>().Result;
                return Json(userList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = new User();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("user/" + id).Result;

                if (response.IsSuccessStatusCode)
                    user = response.Content.ReadAsAsync<User>().Result;
                return View(user);
            }
            catch
            {
                return View(user);
            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            User user = new User();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("user/" + id).Result;

                if (response.IsSuccessStatusCode)
                    user = response.Content.ReadAsAsync<User>().Result;
                return Json(user, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllUserByRoleId(int roleId)
        {
            List<User> userList = new List<User>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("usersByRoleId/"+roleId).Result;

                response.EnsureSuccessStatusCode();
                userList = response.Content.ReadAsAsync<List<User>>().Result;
                return Json(userList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult UserNameExists(string userName)
        {
            string message;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("UserNameExists/" + userName).Result;
                response.EnsureSuccessStatusCode();
                bool exist = response.Content.ReadAsAsync<bool>().Result;
                return Json(exist, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                message = "Server Error";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult UserExistsForEdit(string name, int id)
        {
            string message;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("UserExistsForEdit/" + name + "/" + id).Result;
                response.EnsureSuccessStatusCode();
                bool exist = response.Content.ReadAsAsync<bool>().Result;
                return Json(exist, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                message = "Server Error";
                return Json(message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ContentResult Upload()
        {
            Random r = new Random();
            int rNumber = r.Next(0, 999999);

            HttpPostedFileBase postedFile = Request.Files[0];
            string path = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            postedFile.SaveAs(path + rNumber.ToString() + "_" + postedFile.FileName.ToString());
            Session["ImageNumber"] = rNumber.ToString() + "_" + postedFile.FileName.ToString();

            return Content("Success");
        }

        public string EncodePassword(string pass)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
    }
}