using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class PricingController : Controller
    {
        private string Base_URL = "http://localhost:1848/api/";

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(Pricing price)
        {
            try
            {
                price.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("price", price).Result;
                response.EnsureSuccessStatusCode();
                return Json("Price created", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAll()
        {
            List<Pricing> priceList = new List<Pricing>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("prices").Result;

                response.EnsureSuccessStatusCode();
                priceList = response.Content.ReadAsAsync<List<Pricing>>().Result;
                return Json(priceList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}