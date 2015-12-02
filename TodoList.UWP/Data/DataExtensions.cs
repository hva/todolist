using System.Collections.Generic;
using System.Collections.ObjectModel;
using TodoList.UWP.Models;

namespace TodoList.UWP.Data
{
    public static class DataExtensions
    {
        public static void Merge(this ObservableCollection<object> items, List<Operation> operations)
        {
            foreach (var operation in operations)
            {
                switch (operation.Type)
                {
                    case OperationType.Create:
                        var item = new Item { Text = operation.Value };
                        items.Insert(0, item);
                        break;
                }
            }
        }
    }
}
