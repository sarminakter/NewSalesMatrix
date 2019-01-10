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
    public class PermissionController : Controller
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
        public JsonResult CreateAndUpdate(List<Permission> operationResourceArray)
        {
                if (operationResourceArray != null && operationResourceArray.Count() > 0)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                var ipList = (from ip in host.AddressList where ip.AddressFamily == AddressFamily.InterNetwork select ip.ToString()).ToList();
                var ipAddress = ipList[0].ToString();
                foreach (var item in operationResourceArray)
                {
                    item.CreatedBy = Convert.ToInt32(Session["UserId"].ToString());
                    item.CreatedDate = DateTime.UtcNow;
                    item.CreatedFrom = ipAddress;
                }
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(Base_URL);
                    HttpResponseMessage response = client.PostAsJsonAsync("permissionCreateAndUpdate", operationResourceArray).Result;
                    response.EnsureSuccessStatusCode();
                    return Json("Permission created", JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json("Failed", JsonRequestBehavior.AllowGet);
                }
            }
            else
                return Json("Failed", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Permission permission)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PutAsJsonAsync("permission", permission).Result;
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
            List<Permission> permissionList = new List<Permission>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("permissions").Result;

                response.EnsureSuccessStatusCode();
                permissionList = response.Content.ReadAsAsync<List<Permission>>().Result;
                return Json(permissionList, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetAllByModuleRoleOrUserId(int moduleId, int roleId, int? userId)
        {
            List<Permission> permissionList = new List<Permission>();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("permissionsByModuleRoleOrUserId/" + moduleId+"/"+ roleId + "/"+ userId).Result;

                response.EnsureSuccessStatusCode();
                permissionList = response.Content.ReadAsAsync<List<Permission>>().Result;
                return Json(permissionList, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetById(int id)
        {
            Permission permission = new Permission();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.GetAsync("permission/" + id).Result;
                response.EnsureSuccessStatusCode();
                permission = response.Content.ReadAsAsync<Permission>().Result;
                return Json(permission, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}