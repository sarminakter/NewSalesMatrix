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
    public class OperationController : ApiController
    {
        private OperationService _operationService = new OperationService();

        [HttpPost]
        [Route("api/operation")]
        public void Create(Operation operation)
        {
            _operationService.Create(operation);
        }

        [HttpPut]
        [Route("api/operation")]
        public void Update(Operation operation)
        {
            _operationService.Update(operation);
        }

        [HttpGet]
        [Route("api/operations")]
        public IQueryable<Operation> GetAll()
        {
            var list = _operationService.GetAll();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/operationsbymoduleid/{moduleId}")]
        public IQueryable<Operation> GetAllByModuelId(int moduleId)
        {
            var list = _operationService.GetAllByModuelId(moduleId);
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/operation/{id}")]
        public IHttpActionResult GetById(int id)
        {
            Operation operation = _operationService.GetById(id);
            if (operation == null)
            {
                return NotFound();
            }
            return Ok(operation);
        }

    }
}
