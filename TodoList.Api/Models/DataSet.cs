using System.Collections.Generic;

namespace TodoList.Api.Models
{
    public class DataSet
    {
        public DataSet()
        {
            Items = new List<Item>();
            Operations = new List<Operation>();
        }

        public List<Item> Items { get; set; }
        public List<Operation> Operations { get; set; }
    }
}