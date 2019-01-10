using Newtonsoft.Json;
using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class RoleController : Controller
    {
        //private _SalesMatrixDB db = new _SalesMatrixDB();
        private string Base_URL = "http://localhost:1848/api/";
        //private string Base_URL = "http://192.168.0.113:8003/api/";


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Role role)
        {
            try
            {
                role.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("role", role).Result;
                response.EnsureSuccessStatusCode();
                return Json("Role created", JsonRequestBehavior.AllowGet);
            }
            catch
            {
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
            Role role = new Role();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("role/" + id).Result;
                response.EnsureSuccessStatusCode();
                role = response.Content.ReadAsAsync<Role>().Result;
                return View(role);
            }
            catch
            {
                return View("Index");
            }
        }

        [HttpPost]        
        public ActionResult Edit(Role role)
        {
            
            try
            {
                role.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("role", role).Result;
                response.EnsureSuccessStatusCode();
                return Json("Role Updated", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

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
                HttpResponseMessage response = client.DeleteAsync("role/" + id).Result;
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
            List<Role> roleList = new List<Role>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("roles").Result;

                response.EnsureSuccessStatusCode();
                roleList = response.Content.ReadAsAsync<List<Role>>().Result;
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllActiveRole()
        {
            List<Role> roleList = new List<Role>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("activeRoles").Result;
                response.EnsureSuccessStatusCode();
                roleList = response.Content.ReadAsAsync<List<Role>>().Result;                
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                //throw new Exception();
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllInactiveRole()
        {
            List<Role> roleList = new List<Role>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("inactiveRoles").Result;

                response.EnsureSuccessStatusCode();
                roleList = response.Content.ReadAsAsync<List<Role>>().Result;
                return Json(roleList, JsonRequestBehavior.AllowGet);
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
            Role role = new Role();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("role/" + id).Result;
                response.EnsureSuccessStatusCode();
                role = response.Content.ReadAsAsync<Role>().Result;
                return View(role);
            }
            catch
            {
                return View(role);
            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            Role role = new Role();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("role/" + id).Result;
                response.EnsureSuccessStatusCode();
                role = response.Content.ReadAsAsync<Role>().Result;
                return Json(role, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllUserByRoleId(int roleId)
        {
            List<User> userList = new List<User>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("usersByRoleId/" + roleId).Result;

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
        public JsonResult RoleExists(string name) 
        {
            string message;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("RoleExists/" + name).Result;
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
        public JsonResult RoleExistsForEdit(string name, int id)
        {
            string message;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("RoleExistsForEdit/" + name+"/"+id).Result;
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
    }
}