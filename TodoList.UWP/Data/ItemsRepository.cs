using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.UWP.Data.Interfaces;
using TodoList.UWP.Data.Models;

namespace TodoList.UWP.Data
{
    public class ItemsRepository : IItemsRepository
    {
        private const string fileName = "data.json";

        private readonly IStorageService storageService;
        public ItemsRepository()
        {
            storageService = new StorageService();
        }

        public async Task<List<Item>> GetTodoAsync()
        {
            var set = await LoadItemsSetAsync();
            return set.Todo;
        }

        public async Task<List<Item>> GetDoneAsync()
        {
            var set = await LoadItemsSetAsync();
            return set.Done;
        }

        public async Task CreateAsync(Item item)
        {
            var guid = Guid.NewGuid();
            item.Guid = guid;
            var set = await LoadItemsSetAsync();
            set.Todo.Insert(0, item);
            await SaveItemsSetAsync(set);
        }

        public async Task SetIsDoneAsync(Guid guid, bool isDone)
        {
            var set = await LoadItemsSetAsync();
            if (isDone)
            {
                var item = set.Todo.First(x => x.Guid == guid);
                set.Todo.Remove(item);
                set.Done.Insert(0, item);
            }
            else
            {
                var item = set.Done.First(x => x.Guid == guid);
                set.Done.Remove(item);
                set.Todo.Add(item);
            }
            await SaveItemsSetAsync(set);
        }

        private async Task<ItemsSet> LoadItemsSetAsync()
        {
            var set = await storageService.LoadAsync<ItemsSet>(fileName);
            return set ?? new ItemsSet();
        }

        private Task SaveItemsSetAsync(ItemsSet set)
        {
            return storageService.SaveAsync(set, fileName);
        }
    }
}
