using System;
using System.Collections.Generic;

namespace TodoList.Api.Models
{
    public class DataFeed
    {
        public List<Item> Items { get; set; }
        public Guid? LastOperationId { get; set; }
    }
}