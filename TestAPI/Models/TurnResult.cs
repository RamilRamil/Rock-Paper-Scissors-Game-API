using System;
using Newtonsoft.Json;

namespace TestAPI.Models
{
    public class TurnResult
    {
        [JsonProperty("isGameOver")]
        public bool IsGameOver { get; set; }
        [JsonProperty("roundResult")]
        public RoundResult RoundResult { get; set; }
        [JsonProperty("GameResult")]
        public GameResult? GameResult { get; set; }
    }
}

