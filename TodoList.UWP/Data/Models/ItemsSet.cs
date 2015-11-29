using System;
using System.Collections.Generic;

namespace TodoList.UWP.Data.Models
{
    public class ItemsSet
    {
        public ItemsSet()
        {
            Todo = new List<Item>();
            Done = new List<Item>();
        }

        public List<Item> Todo { get; set; }
        public List<Item> Done { get; set; }
    }
}
