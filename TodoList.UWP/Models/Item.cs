using System;

namespace TodoList.UWP.Models
{
    public class Item
    {
        public Guid Guid { get; set; }
        public string Text { get; set; }
        public bool IsDone { get; set; }
    }
}
