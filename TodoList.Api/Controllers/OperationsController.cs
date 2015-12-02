using System;
using System.Web.Http;
using TodoList.Api.Data;
using TodoList.Api.Models;

namespace TodoList.Api.Controllers
{
    public class OperationsController : ApiController
    {
        [CacheControl(MaxAge = 5)]
        public IHttpActionResult Get(Guid? lastOperationId)
        {
            var data = StorageHelper.Read<DataSet>(Constants.FileName) ?? new DataSet();
            var operations = data.GetOperationsSince(lastOperationId);
            return Ok(operations);
        }

        public IHttpActionResult Post(Operation operation, Guid? lastOperationId)
        {
            var data = StorageHelper.Read<DataSet>(Constants.FileName) ?? new DataSet();
            data.Merge(operation);
            StorageHelper.Write(data, Constants.FileName);
            var operations = data.GetOperationsSince(lastOperationId);
            return Created(string.Empty, operations);
        }
    }
}
