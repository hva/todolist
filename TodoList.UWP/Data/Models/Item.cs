using System;

namespace TodoList.UWP.Data.Models
{
    public class Item
    {
        public Guid Guid { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
    }
}
