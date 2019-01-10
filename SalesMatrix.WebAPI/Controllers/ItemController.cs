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
    public class ItemController : ApiController
    {
        private ItemService _itemService = new ItemService();

        [HttpPost]
        [Route("api/item")]
        public void Create(Item item)
        {
            _itemService.Create(item);
        }

        [HttpPut]
        [Route("api/item")]
        public void Update(Item item)
        {
            _itemService.Update(item);
        }

        [HttpGet]
        [Route("api/items")]
        public IQueryable<Item> GetAll()
        {
            var list = _itemService.GetAll();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/activeitems")]
        public IQueryable<Item> GetAllActiveUser()
        {
            List<Item> list = _itemService.GetAllActiveItem();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/inactiveitems")]
        public IQueryable<Item> GetAllInactiveUser()
        {
            List<Item> list = _itemService.GetAllInactiveItem();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/actualitems")]
        public IQueryable<Item> GetAllActualItem()
        {
            List<Item> list = _itemService.GetAllActualItem();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/notactualitems")]
        public IQueryable<Item> GetAllNotActualItem()
        {
            List<Item> list = _itemService.GetAllNotActualItem();
            return list.AsQueryable();
        }

        [HttpGet]
        [Route("api/item/{id}")]
        public IHttpActionResult GetById(int id)
        {
            Item item = _itemService.GetById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
    }
}
