using SalesMatrix.Entity.Models;
using SalesMatrix.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SalesMatrix.Controllers
{
    public class HomeController : Controller
    {
        private string Base_URL = "http://localhost:1848/api/";
        //private string Base_URL = "http://192.168.0.113:8003/api/";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(User user)
        {
            EnsureLoggedOut();
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(Base_URL);
                HttpResponseMessage response = client.PostAsJsonAsync("validLogin", user).Result;
                response.EnsureSuccessStatusCode();
                bool result = response.Content.ReadAsAsync<bool>().Result;
                if (result == true)
                {
                    HttpResponseMessage response1 = client.GetAsync("userbyName/" + user.UserName).Result;
                    response1.EnsureSuccessStatusCode();
                    User gotUser = response1.Content.ReadAsAsync<User>().Result;
                    Session["User"] = gotUser.UserName;
                    Session["UserId"] = gotUser.Id;

                    return Json("ok", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("invalid", JsonRequestBehavior.AllowGet);
                }

            }
            catch
            {
                return Json("Failed", JsonRequestBehavior.AllowGet);
            }
        }

        //GET: EnsureLoggedOut
        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }


        public ActionResult Logout()
        {
            try
            {
                // First we clean the authentication ticket like always
                //required NameSpace: using System.Web.Security;
                FormsAuthentication.SignOut();

                // Second we clear the principal to ensure the user does not retain any authentication
                //required NameSpace: using System.Security.Principal;
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

                Session.Clear();
                System.Web.HttpContext.Current.Session.RemoveAll();

                // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
                // this clears the Request.IsAuthenticated flag since this triggers a new request
                return RedirectToAction("Login");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}