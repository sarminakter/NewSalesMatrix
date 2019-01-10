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
    public class ModuleController : ApiController
    {
        private ModuleService _moduleService = new ModuleService();

        [HttpPost]
        [Route("api/module")]
        public void Create(Module module)
        {
            _moduleService.Create(module);
        }

        [HttpPut]
        [Route("api/module")]
        public void Update(Module module)
        {
            _moduleService.Update(module);
        }

        [HttpGet]
        [Route("api/modules")]
        public IQueryable<Module> GetAll()
        {
            var ModuleList = _moduleService.GetAll();
            return ModuleList.AsQueryable();
        }

        [HttpGet]
        [Route("api/activeModules")]
        public IQueryable<Module> GetAllActiveModule()
        {
            var ModuleList = _moduleService.GetAllActiveModule();
            return ModuleList.AsQueryable();
        }

        [HttpGet]
        [Route("api/inactiveModules")]
        public IQueryable<Module> GetAllInactiveModule()
        {
            var ModuleList = _moduleService.GetAllInactiveModule();
            return ModuleList.AsQueryable();
        }

        [HttpGet]
        [Route("api/module/{id}")]
        public IHttpActionResult GetById(int id)
        {
            Module module = _moduleService.GetById(id);
            if (module == null)
            {
                return NotFound();
            }
            return Ok(module);
        }

        [HttpGet]
        [Route("api/ModuleExists/{name}")]
        public JsonResult<bool> ModuleExists(string name)
        {
            return Json(_moduleService.ModuleExists(name));
        }

        [HttpGet]
        [Route("api/ModuleExistsForEdit/{name}/{id}")]
        public JsonResult<bool> ModuleExistsForEdit(string name, int id)
        {
            return Json(_moduleService.ModuleExistsForEdit(name, id));
        }
        //[HttpDelete]
        //[Route("api/module/{id}")]
        //public void Delete(int id)
        //{
        //    _moduleService.Delete(id);
        //}
    }
}
