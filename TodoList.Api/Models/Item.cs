using System;

namespace TodoList.Api.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}
