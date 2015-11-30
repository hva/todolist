using System;
using System.Collections.Generic;
using System.Web.Http;
using TodoList.Api.Data;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class ItemsController : ApiController
    {
        private const string fileName = "data.json";
        private readonly List<Item> list;

        public ItemsController()
        {
            list = StorageHelper.Read<List<Item>>(fileName) ?? new List<Item>();
        }

        public IHttpActionResult Get()
        {
            return Ok(list);
        }

        public IHttpActionResult Post(Item item)
        {
            item.Guid = Guid.NewGuid();
            list.Insert(0, item);
            Flush();
            return Created(string.Empty, item.Guid);
        }

        private void Flush()
        {
            StorageHelper.Write(list, fileName);
        }
    }
}
