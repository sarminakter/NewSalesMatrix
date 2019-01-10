using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class OperationController : Controller
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
        public JsonResult Create(Operation operation)
        {
            try
            {
                operation.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("operation", operation).Result;
                response.EnsureSuccessStatusCode();
                return Json("Operation created", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(Operation operation)
        {
            try
            {
                operation.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("operation", operation).Result;
                response.EnsureSuccessStatusCode();
                return Json("Operation Updated", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            List<Operation> operationList = new List<Operation>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("operations").Result;

                response.EnsureSuccessStatusCode();
                operationList = response.Content.ReadAsAsync<List<Operation>>().Result;
                return Json(operationList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllByModuelId(int moduleId)
        {
            List<Operation> operationList = new List<Operation>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("operationsbymoduleid/"+moduleId).Result;

                response.EnsureSuccessStatusCode();
                operationList = response.Content.ReadAsAsync<List<Operation>>().Result;
                return Json(operationList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            Operation operation = new Operation();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("operation/" + id).Result;
                response.EnsureSuccessStatusCode();
                operation = response.Content.ReadAsAsync<Operation>().Result;
                return Json(operation, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}