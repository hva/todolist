using System;
using System.Collections.Generic;

namespace TodoList.UWP.Data.Models
{
    public class ItemsSet
    {
        public ItemsSet()
        {
            Todos = new List<Item>();
            Done = new List<Item>();
            TodosSortorder = new List<Guid>();
            DoneSortorder = new List<Guid>();
        }

        public List<Item> Todos { get; set; }
        public List<Item> Done { get; set; }
        public List<Guid> TodosSortorder { get; set; }
        public List<Guid> DoneSortorder { get; set; }
    }
}
