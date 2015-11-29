using System;
using System.Collections.Generic;

namespace TodoList.UWP.Data.Models
{
    public class ItemsSet
    {
        public ItemsSet()
        {
            Items = new List<Item>();
            Sortorder = new List<Guid>();
        }

        public List<Item> Items { get; set; }
        public List<Guid> Sortorder { get; set; }
    }
}
