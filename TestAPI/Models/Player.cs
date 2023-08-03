using System;
using Newtonsoft.Json;

namespace TestAPI.Models
{
    public class Player
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}

