using System;
using Newtonsoft.Json;

namespace TestAPI.Models
{
    public class Round
    {
        [JsonProperty("player1Turn")]
        public Turn? Player1Turn { get; set; }
        [JsonProperty("player2Turn")]
        public Turn? Player2Turn { get; set; }
        [JsonProperty("result")]
        public RoundResult Result { get; set; }
    }
}

