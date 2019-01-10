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
    public class PricingController : ApiController
    {
        private PricingService _priceService = new PricingService();

        [HttpPost]
        [Route("api/Price")]
        public void Create(Pricing price)
        {
            _priceService.Create(price);
        }

        [HttpGet]
        [Route("api/Prices")]
        public IQueryable<Pricing> GetAll()
        {
            var priceList = _priceService.GetAll();
            return priceList.AsQueryable();
        }
    }
}
