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
    public class RetailCustomerController : ApiController
    {
        private RetailCustomerService _retailCustomerService = new RetailCustomerService();

        [HttpPost]
        [Route("api/customer")]
        public void Create(RetailCustomer customer)
        {
            _retailCustomerService.Create(customer);
        }

        [HttpPut]
        [Route("api/customer")]
        public void Update(RetailCustomer customer)
        {
            _retailCustomerService.Update(customer);
        }

        [HttpGet]
        [Route("api/customers")]
        public IQueryable<RetailCustomer> GetAll()
        {
            var list = _retailCustomerService.GetAll();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/customer/{id}")]
        public IHttpActionResult GetById(int id)
        {
            RetailCustomer customer = _retailCustomerService.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}
