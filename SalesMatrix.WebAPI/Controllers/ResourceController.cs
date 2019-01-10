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
    public class ResourceController : ApiController
    {
        private ResourceService _resourceService = new ResourceService();

        [HttpPost]
        [Route("api/resource")]
        public void Create(Resource resource)
        {
            _resourceService.Create(resource);
        }

        [HttpPut]
        [Route("api/resource")]
        public void Update(Resource resource)
        {
            _resourceService.Update(resource);
        }

        [HttpGet]
        [Route("api/resources")]
        public IQueryable<Resource> GetAll()
        {
            var list = _resourceService.GetAll();            
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/resourcesbymoduleid/{moduleId}")]
        public IQueryable<Resource> GetAllByModuelId(int moduleId)
        {
            var list = _resourceService.GetAllByModuelId(moduleId);
            return list.AsQueryable();
        }

        
    
    [HttpGet]
        [Route("api/resource/{id}")]
        public IHttpActionResult GetById(int id)
        {
            Resource resource = _resourceService.GetById(id);
            if (resource == null)
            {
                return NotFound();
            }
            return Ok(resource);
        }
    }
}
