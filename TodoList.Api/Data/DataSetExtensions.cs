using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Api.Models;

namespace TodoList.Api.Data
{
    public static class DataSetExtensions
    {
        // Merges single operation to items list
        // and adds operation to operations list.
        public static void Merge(this DataSet data, Operation operation)
        {
            switch (operation.Type)
            {
                case OperationType.Create:
                    data.CreateItem(operation);
                    break;
                case OperationType.Reorder:
                    data.ReorderItem(operation);
                    break;
                case OperationType.ChangeStatus:
                    data.ChangeItemStatus(operation);
                    break;
            }
        }

        // Returns list of operations since 'lastOperationId'.
        public static List<Operation> GetOperationsSince(this DataSet data, Guid? lastOperationId)
        {
            if (lastOperationId.HasValue)
            {
                return data.Operations.SkipWhile(x => x.Id != lastOperationId.Value).Skip(1).ToList();
            }
            return data.Operations;
        }

        // Creates new item, puts it to the top of items list
        // and adds operation to operations list.
        private static void CreateItem(this DataSet data, Operation operation)
        {
            var item = new Item
            {
                Id = Guid.NewGuid(),
                Text = operation.Value
            };
            data.Items.Insert(0, item);

            operation.Id = Guid.NewGuid();
            operation.ItemId = item.Id;
            data.Operations.Add(operation);
        }

        // Moves existing item to specified position
        // and adds operation to operations list.
        private static void ReorderItem(this DataSet data, Operation operation)
        {
            if (string.IsNullOrEmpty(operation.Value)) return;

            int newIndex;
            if (int.TryParse(operation.Value, out newIndex))
            {
                var item = data.Items.FirstOrDefault(x => x.Id == operation.ItemId);
                if (item != null)
                {
                    var oldIndex = data.Items.IndexOf(item);
                    if (oldIndex != newIndex)
                    {
                        data.Items.RemoveAt(oldIndex);
                        data.Items.Insert(newIndex, item);

                        operation.Id = Guid.NewGuid();
                        data.Operations.Add(operation);
                    }
                }
            }
        }

        // Changes 'IsCompleted' status of existing item
        // and adds operation to operations list.
        private static void ChangeItemStatus(this DataSet data, Operation operation)
        {
            if (string.IsNullOrEmpty(operation.Value)) return;

            bool newValue;
            if (bool.TryParse(operation.Value, out newValue))
            {
                var item = data.Items.FirstOrDefault(x => x.Id == operation.ItemId);
                if (item != null)
                {
                    item.IsComplete = newValue;

                    data.Items.Remove(item);
                    var index = data.Items.Count(x => !x.IsComplete);
                    data.Items.Insert(index, item);

                    operation.Id = Guid.NewGuid();
                    data.Operations.Add(operation);
                }
            }
        }
    }
}