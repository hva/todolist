using System;
using Newtonsoft.Json;

namespace TodoList.UWP.Models
{
    
    public class Operation
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid Id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Guid ItemId { get; set; }

        public OperationType Type { get; set; }

        public string Value { get; set; }
    }
}