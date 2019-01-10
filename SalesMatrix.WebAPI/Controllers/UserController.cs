using SalesMatrix.Entity.Models;
using SalesMatrix.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace SalesMatrix.WebAPI.Controllers
{
    public class UserController : ApiController
    {
        private UserInfoService _userService = new UserInfoService();

        [HttpPost]
        [Route("api/user")]
        public void Create(User user)
        {
            _userService.Create(user);
        }

        [HttpPut]
        [Route("api/user")]
        public void Update(User user)
        {
            _userService.Update(user);
        }

        [HttpDelete]
        [Route("api/user/{id}")]
        public void Delete(int id)
        {
            _userService.Delete(id);
        }

        [HttpGet]
        [Route("api/users")]
        public IQueryable<User> GetAll()
        {
            List<User> list = _userService.GetAll();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/user/{id}")]
        public IHttpActionResult GetById(int id)
        {
            User user = _userService.GetById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet]
        [Route("api/userbyName/{userName}")]
        public IHttpActionResult GetByUserName(string userName)
        {
            User user = _userService.GetByUserName(userName);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("api/activeUsers")]
        public IQueryable<User> GetAllActiveUser()
        {
            List<User> list = _userService.GetAllActiveUser();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/inactiveUsers")]
        public IQueryable<User> GetAllInactiveUser()
        {
            List<User> list = _userService.GetAllInactiveUser();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/usersByRoleId/{roleId}")]
        public IQueryable<User> GetAllUserByRoleId(int roleId)
        {
            List<User> list = _userService.GetAllUserByRoleId(roleId);
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/UserNameExists/{userName}")]
        public JsonResult<bool> UserNameExists(string userName)
        {
            return Json(_userService.UserNameExists(userName));
        }


        [HttpGet]
        [Route("api/UserExistsForEdit/{name}/{id}")]
        public JsonResult<bool> UserExistsForEdit(string name, int id)
        {
            return Json(_userService.UserExistsForEdit(name, id));
        }
    }
}
