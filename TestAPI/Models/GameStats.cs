using System;
using Newtonsoft.Json;

namespace TestAPI.Models
{
    public class GameStats
    {
        [JsonProperty("gameId")]
        public Guid GameId { get; set; }

        [JsonProperty("player1Wins")]
        public int Player1Wins { get; set; }

        [JsonProperty("player2Wins")]
        public int Player2Wins { get; set; }

        [JsonProperty("draws")]
        public int Draws { get; set; }
    }

}

