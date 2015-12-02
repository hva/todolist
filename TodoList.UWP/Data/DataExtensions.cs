using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TodoList.UWP.Models;
using TodoList.UWP.ViewModels.MainPage;

namespace TodoList.UWP.Data
{
    public static class DataExtensions
    {
        public static void Init(this ObservableCollection<ItemViewModel> collection, IEnumerable<Item> items, Action<Item> onStatusChanged)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var viewModel = item.ToViewModel(onStatusChanged);
                collection.Add(viewModel);
            }
        }

        public static void Merge(this ObservableCollection<ItemViewModel> items, List<Operation> operations, Action<Item> onStatusChanged)
        {
            foreach (var operation in operations)
            {
                switch (operation.Type)
                {
                    case OperationType.Create:
                        items.CreateItem(operation, onStatusChanged);
                        break;
                    case OperationType.Reorder:
                        items.MoveItem(operation);
                        break;
                    case OperationType.ChangeStatus:
                        items.ChangeItemStatus(operation);
                        break;
                }
            }
        }

        private static void CreateItem(this IList<ItemViewModel> items, Operation operation, Action<Item> onIsCompleteChanged)
        {
            var itemExists = items.Any(x => x.Item.Id == operation.ItemId);
            if (itemExists) return;

            var item = new Item
            {
                Id = operation.ItemId,
                Text = operation.Value,
            };
            var viewModel = item.ToViewModel(onIsCompleteChanged);
            items.Insert(0, viewModel);
        }

        private static void MoveItem(this IList<ItemViewModel> items, Operation operation)
        {
            if (string.IsNullOrEmpty(operation.Value)) return;

            int newIndex;
            if (int.TryParse(operation.Value, out newIndex))
            {
                var item = items.FirstOrDefault(x => x.Item.Id == operation.ItemId);
                if (item != null)
                {
                    var oldIndex = items.IndexOf(item);
                    if (oldIndex != newIndex)
                    {
                        items.RemoveAt(oldIndex);
                        items.Insert(newIndex, item);
                    }
                }
            }
        }

        private static void ChangeItemStatus(this IList<ItemViewModel> items, Operation operation)
        {
            if (string.IsNullOrEmpty(operation.Value)) return;

            bool newValue;
            if (bool.TryParse(operation.Value, out newValue))
            {
                var item = items.FirstOrDefault(x => x.Item.Id == operation.ItemId);
                if (item != null)
                {
                    item.SetIsComplete(newValue);

                    items.Remove(item);
                    var index = items.Count(x => !x.Item.IsComplete);
                    items.Insert(index, item);
                }
            }
        }

        private static ItemViewModel ToViewModel(this Item item, Action<Item> onIsCompleteChanged)
        {
            return new ItemViewModel
            {
                Item = item,
                OnStatusChanged = onIsCompleteChanged
            };
        }
    }
}
