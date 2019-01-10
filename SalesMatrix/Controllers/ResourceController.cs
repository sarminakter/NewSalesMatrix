using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class ResourceController : Controller
    {
        private string Base_URL = "http://localhost:1848/api/";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Resource resource)
        {            
            try
            {
                resource.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("resource", resource).Result;
                response.EnsureSuccessStatusCode();
                return Json("Resource created", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(Resource resource)
        {
            try
            {
                resource.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("resource", resource).Result;
                response.EnsureSuccessStatusCode();
                return Json("Role Updated", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            List<Resource> resourceList = new List<Resource>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("resources").Result;

                response.EnsureSuccessStatusCode();
                resourceList = response.Content.ReadAsAsync<List<Resource>>().Result;
                return Json(resourceList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllByModuelId(int moduleId)
        {
            List<Resource> resourceList = new List<Resource>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("resourcesbymoduleid/"+moduleId).Result;

                response.EnsureSuccessStatusCode();
                resourceList = response.Content.ReadAsAsync<List<Resource>>().Result;
                return Json(resourceList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetById(int id)
        {
            Resource resource = new Resource();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("resource/" + id).Result;
                response.EnsureSuccessStatusCode();
                resource = response.Content.ReadAsAsync<Resource>().Result;
                return Json(resource, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}