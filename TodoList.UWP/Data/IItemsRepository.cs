using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.UWP.Models;

namespace TodoList.UWP.Data
{
    public interface IItemsRepository
    {
        Task<List<Item>> GetAsync();
        Task CreateAsync(Item item);
        Task SetIsDoneAsync(Guid guid, bool isDone);
    }
}
