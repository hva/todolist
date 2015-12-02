﻿using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Api.Models;

namespace TodoList.Api.Data
{
    public static class DataSetExtensions
    {
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

        public static List<Operation> GetOperationsSince(this DataSet data, Guid? lastOperationId)
        {
            if (lastOperationId.HasValue)
            {
                return data.Operations.SkipWhile(x => x.Id != lastOperationId.Value).Skip(1).ToList();
            }
            return data.Operations;
        }

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

                    operation.Id = Guid.NewGuid();
                    data.Operations.Add(operation);
                }
            }
        }
    }
}