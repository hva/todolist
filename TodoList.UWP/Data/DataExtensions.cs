using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TodoList.UWP.Models;

namespace TodoList.UWP.Data
{
    public static class DataExtensions
    {
        public static void Merge(this ObservableCollection<Item> items, List<Operation> operations)
        {
            foreach (var operation in operations)
            {
                switch (operation.Type)
                {
                    case OperationType.Create:
                        items.CreateItem(operation);
                        break;
                    case OperationType.Reorder:
                        items.MoveItem(operation);
                        break;
                }
            }
        }

        private static void CreateItem(this IList<Item> items, Operation operation)
        {
            var item = new Item
            {
                Id = operation.ItemId,
                Text = operation.Value,
            };
            items.Insert(0, item);
        }

        private static void MoveItem(this IList<Item> items, Operation operation)
        {
            if (string.IsNullOrEmpty(operation.Value)) return;

            int newIndex;
            if (int.TryParse(operation.Value, out newIndex))
            {
                var item = items.FirstOrDefault(x => x.Id == operation.ItemId);
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
    }
}
