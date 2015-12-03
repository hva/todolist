using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.UWP.Models;

namespace TodoList.UWP.Data
{
    public interface IDataRepository
    {
        // Returns DataSet with list of items and last operation id.
        Task<DataFeed> GetFeedAsync();

        // Returns list of operations since 'lastOperationId' parameter.
        Task<List<Operation>> GetOperationsAsync(Guid? lastOperationId);

        // Creates new operation and returns operations list since 'lastOperationId' parameter.
        Task<List<Operation>> PostOperationsAsync(Guid? lastOperationId, Operation operation);
    }
}
