using System;
using TestAPI.Models;

namespace TestAPI.Services
{
    public interface IGameService
    {
        Task<Game> CreateGameAsync(string userName1);
        Task<Guid> JoinGameAsync(Guid gameId, string userName2);
        Task<TurnResult> MakeTurnAsync(Guid gameId, Guid userId, Turn turn);
        Task<GameStats> GetGameStatsAsync(Guid gameId);
        Task CancelGameAsync(Guid gameId);
    }
}

