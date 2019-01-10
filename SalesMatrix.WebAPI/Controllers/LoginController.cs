using SalesMatrix.Entity.Models;
using SalesMatrix.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SalesMatrix.WebAPI.Controllers
{
    public class LoginController : ApiController
    {
        LoginService _loginService = new LoginService();
        [HttpPost]
        [Route("api/validLogin")]
        public bool ValidLogin(User user)
        {
            return _loginService.ValidLogin(user);
        }
    }
}
