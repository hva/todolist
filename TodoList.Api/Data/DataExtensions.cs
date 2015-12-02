using System;
using System.Collections.Generic;
using System.Linq;
using TodoList.Api.Models;

namespace TodoList.Api.Data
{
    public static class DataExtensions
    {
        public static void Merge(this DataSet data, Operation operation)
        {
            switch (operation.Type)
            {
                case OperationType.Create:

                    var item = new Item
                    {
                        Id = Guid.NewGuid(),
                        Text = operation.Value
                    };
                    data.Items.Insert(0, item);

                    operation.Id = Guid.NewGuid();
                    operation.ItemId = item.Id;
                    data.Operations.Add(operation);
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
    }
}