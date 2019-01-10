using SalesMatrix.Entity.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace SalesMatrix.Controllers
{
    public class ItemController : Controller
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
        public JsonResult Create(Item item)
        {
            if (Session["ItemImageNumber"] != null)
            {
                item.Picture = Session["ItemImageNumber"].ToString();
            }
            try
            {
                item.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("item", item).Result;
                if (response.IsSuccessStatusCode)
                {
                    Session["ItemImageNumber"] = null;
                    return Json("Item created", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (Session["ItemImageNumber"] != null)
                    {
                        string fullPath = Request.MapPath("~/Uploads/" + Session["ItemImageNumber"].ToString());
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    Session["ItemImageNumber"] = null;
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                Session["ItemImageNumber"] = null;
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
            Item item = new Item();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("item/" + id).Result;
                response.EnsureSuccessStatusCode();
                item = response.Content.ReadAsAsync<Item>().Result;
                return View(item);
            }
            catch
            {
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            try
            {
                item.ModifiedBy = Convert.ToInt32(Session["UserId"].ToString());
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("item", item).Result;
                response.EnsureSuccessStatusCode();
                return Json("Item Updated", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetAll()
        {
            List<Item> itemList = new List<Item>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("items").Result;

                response.EnsureSuccessStatusCode();
                itemList = response.Content.ReadAsAsync<List<Item>>().Result;
                return Json(itemList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllActiveItem()
        {
            List<Item> itemList = new List<Item>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("activeitems").Result;

                response.EnsureSuccessStatusCode();
                itemList = response.Content.ReadAsAsync<List<Item>>().Result;
                return Json(itemList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllInactiveItem()
        {
            List<Item> itemList = new List<Item>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("inactiveitems").Result;

                response.EnsureSuccessStatusCode();
                itemList = response.Content.ReadAsAsync<List<Item>>().Result;
                return Json(itemList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllActualItem()
        {
            List<Item> itemList = new List<Item>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("actualitems").Result;

                response.EnsureSuccessStatusCode();
                itemList = response.Content.ReadAsAsync<List<Item>>().Result;
                return Json(itemList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllNotActualItem()
        {
            List<Item> itemList = new List<Item>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("notactualitems").Result;

                response.EnsureSuccessStatusCode();
                itemList = response.Content.ReadAsAsync<List<Item>>().Result;
                return Json(itemList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            Item item = new Item();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("item/" + id).Result;
                response.EnsureSuccessStatusCode();
                item = response.Content.ReadAsAsync<Item>().Result;
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
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

            postedFile.SaveAs(path + rNumber.ToString() + "_item_" + postedFile.FileName.ToString());
            Session["ItemImageNumber"] = rNumber.ToString() + "_item_" + postedFile.FileName.ToString();

            return Content("Success");
        }
    }
}