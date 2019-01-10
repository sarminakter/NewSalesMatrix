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
    public class PermissionController : ApiController
    {
        private PermissionService _permissionService = new PermissionService();

        [HttpPost]
        [Route("api/permissionCreateAndUpdate")]
        public void CreateAndUpdate(List<Permission> permission)
        {
            _permissionService.CreateAndUpdate(permission);
        }

        [HttpPut]
        [Route("api/permission")]
        public void Update(Permission permission)
        {
            _permissionService.Update(permission);
        }

        [HttpGet]
        [Route("api/permissions")]
        public IQueryable<Permission> GetAll()
        {
            var list = _permissionService.GetAll();
            return list.AsQueryable();
        }

        //[HttpGet]
        //[Route("api/permissionsByModuleId/{moduleId}")]
        //public IQueryable<Permission> GetAllByModuleId(int moduleId)
        //{
        //    var list = _permissionService.GetAllByModuleId(moduleId);
        //    return list.AsQueryable();
        //}

        [HttpGet]
        [Route("api/permissionsByModuleRoleOrUserId/{moduleId}/{roleId}/{userId:int?}")]
        public IQueryable<Permission> GetAllByModuleRoleOrUserId(int moduleId, int roleId, int? userId=null)
        {
            var list = _permissionService.GetAllByModuleRoleOrUserId(moduleId,roleId,userId);
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/permission/{id}")]
        public IHttpActionResult GetById(int id)
        {
            Permission permission = _permissionService.GetById(id);
            if (permission == null)
            {
                return NotFound();
            }
            return Ok(permission);
        }
    }
}
