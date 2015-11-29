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
        public ItemsRepository(IStorageService storageService)
        {
            this.storageService = storageService;
        }

        public async Task<List<Item>> GetTodosAsync()
        {
            var set = await LoadItemsSetAsync();
            return GetSortedList(set.Todos, set.TodosSortorder);
        }

        public async Task<List<Item>> GetDoneAsync()
        {
            var set = await LoadItemsSetAsync();
            return GetSortedList(set.Done, set.DoneSortorder);
        }

        public async Task CreateAsync(Item item)
        {
            var guid = Guid.NewGuid();
            item.Guid = guid;
            var set = await LoadItemsSetAsync();
            set.TodosSortorder.Insert(0, guid);
            set.Todos.Add(item);
            await SaveItemsSetAsync(set);
        }

        private static List<Item> GetSortedList(IEnumerable<Item> list, IEnumerable<Guid> sortorder)
        {
            var hash = list.ToDictionary(x => x.Guid);
            return sortorder.Select(key => hash[key]).ToList();
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
