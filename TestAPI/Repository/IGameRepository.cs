using System;
using TestAPI.Models;

namespace TestAPI.Repository
{
    public interface IGameRepository
    {
        Task<Game> GetAsync(Guid id);
        Task SaveAsync(Game game);
        Task UpdateAsync(Game game);
        Task DeleteAsync(Guid gameId);
    }

}

