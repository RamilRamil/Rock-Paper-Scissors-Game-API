using System;
using Newtonsoft.Json;

namespace TestAPI.Models
{
    public class Game
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("player1")]
        public Player Player1 { get; set; }

        [JsonProperty("player2")]
        public Player? Player2 { get; set; }

        [JsonProperty("currentRound")]
        public Round? CurrentRound { get; set; }

        [JsonProperty("rounds")]
        public List<Round> Rounds { get; set; }

        [JsonProperty("status")]
        public GameStatus Status { get; set; }

        [JsonProperty("result")]
        public GameResult? Result { get; set; }

        [JsonProperty("stats")]
        public GameStats? GameStats { get; set; }

        [JsonProperty("isAborted")]
        public bool IsAborted { get; private set; }

        [JsonProperty("isOver")]
        public bool IsOver
        {
            get
            {
                return Player2 != null && (Rounds.Count >= 5 || IsAborted);
            }
        }

        public void Abort()
        {
            IsAborted = true;
        }

        public Game(Player player)
        {
            Player1 = player;
            Id = Guid.NewGuid();
            Rounds = new List<Round>();
        }
    }

}

