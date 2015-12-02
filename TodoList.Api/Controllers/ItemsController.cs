using System;
using System.Linq;
using System.Web.Http;
using TodoList.Api.Data;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class ItemsController : ApiController
    {
        public IHttpActionResult Get()
        {
            var data = StorageHelper.Read<DataSet>(Constants.FileName) ?? new DataSet();

            Guid? lastOperationId = null;
            if (data.Operations.Count > 0)
            {
                lastOperationId = data.Operations.Last().Id;
            }

            var feed = new DataFeed
            {
                Items = data.Items,
                LastOperationId = lastOperationId
            };
            return Ok(feed);
        }
    }
}
