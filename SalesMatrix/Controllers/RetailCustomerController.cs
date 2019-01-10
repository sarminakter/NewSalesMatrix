using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class RetailCustomerController : Controller
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
        public JsonResult Create(RetailCustomer customer)
        {
            try
            {
                customer.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("customer", customer).Result;
                response.EnsureSuccessStatusCode();
                return Json("Customer created", JsonRequestBehavior.AllowGet);
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
            RetailCustomer customer = new RetailCustomer();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("customer/" + id).Result;
                response.EnsureSuccessStatusCode();
                customer = response.Content.ReadAsAsync<RetailCustomer>().Result;
                return View(customer);
            }
            catch
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(RetailCustomer customer)
        {
            try
            {
                customer.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("customer", customer).Result;
                response.EnsureSuccessStatusCode();
                return Json("Customer Updated", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            List<RetailCustomer> customerList = new List<RetailCustomer>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("customers").Result;

                response.EnsureSuccessStatusCode();
                customerList = response.Content.ReadAsAsync<List<RetailCustomer>>().Result;
                return Json(customerList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            RetailCustomer customer = new RetailCustomer();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("customer/" + id).Result;
                response.EnsureSuccessStatusCode();
                customer = response.Content.ReadAsAsync<RetailCustomer>().Result;
                return Json(customer, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}