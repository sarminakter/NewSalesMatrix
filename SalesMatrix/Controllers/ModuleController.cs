using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class ModuleController : Controller
    {
        private string Base_URL = "http://localhost:1848/api/";

        public ActionResult Create()    
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Module module)
        {
            try
            {
                module.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("module", module).Result;
                response.EnsureSuccessStatusCode();
                return Json("Module created", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAll()
        {
            List<Module> moduleList = new List<Module>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("modules").Result;

                response.EnsureSuccessStatusCode();
                moduleList = response.Content.ReadAsAsync<List<Module>>().Result;
                return Json(moduleList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllActiveModule()
        {
            List<Module> moduleList = new List<Module>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("activeModules").Result;

                response.EnsureSuccessStatusCode();
                moduleList = response.Content.ReadAsAsync<List<Module>>().Result;
                return Json(moduleList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllInactiveModule()
        {
            List<Module> moduleList = new List<Module>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("inactiveModules").Result;

                response.EnsureSuccessStatusCode();
                moduleList = response.Content.ReadAsAsync<List<Module>>().Result;
                return Json(moduleList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ModuleExists(string name)
        {
            string message;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("ModuleExists/" + name).Result;
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