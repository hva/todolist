using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.UWP.Data.Models;

namespace TodoList.UWP.Data.Interfaces
{
    public interface IItemsRepository
    {
        Task<List<Item>> GetTodosAsync();
        Task<List<Item>> GetDoneAsync();
        Task CreateAsync(Item item);
    }
}
