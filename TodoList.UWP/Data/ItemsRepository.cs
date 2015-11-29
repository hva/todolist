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

        public Task<List<Item>> GetTodosAsync()
        {
            return GetOrderedItems(x => !x.IsDone);
        }

        public Task<List<Item>> GetDoneAsync()
        {
            return GetOrderedItems(x => x.IsDone);
        }

        public async Task CreateAsync(Item item)
        {
            var guid = Guid.NewGuid();
            item.Guid = guid;
            var set = await LoadItemsSetAsync();
            set.Sortorder.Insert(0, guid);
            set.Items.Add(item);
            await SaveItemsSetAsync(set);
        }

        private async Task<List<Item>> GetOrderedItems(Predicate<Item> isValid)
        {
            var set = await LoadItemsSetAsync();
            var hash = set.Items.ToDictionary(x => x.Guid, x => x);
            return set.Sortorder.Select(key => hash[key]).Where(val => isValid(val)).ToList();
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
