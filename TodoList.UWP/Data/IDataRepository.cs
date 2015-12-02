using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.UWP.Models;

namespace TodoList.UWP.Data
{
    public interface IDataRepository
    {
        Task<DataFeed> GetFeedAsync();
        Task<List<Operation>> GetOperationsAsync(Guid? lastOperationId);
        Task<List<Operation>> PostOperationsAsync(Guid? lastOperationId, Operation operation);
    }
}
