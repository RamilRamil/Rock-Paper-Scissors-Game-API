using System;
using Newtonsoft.Json;
using TestAPI.Models;

namespace TestAPI.Repository
{
    public class FileGameRepository : IGameRepository
    {
        private const string FilePath = "games.json";
        private readonly SemaphoreSlim _syncRoot = new SemaphoreSlim(1, 1);

        public async Task<Game> GetAsync(Guid id)
        {
            await _syncRoot.WaitAsync();
            try
            {
                if (!File.Exists(FilePath))
                {
                    return null;
                }

                var jsonData = await File.ReadAllTextAsync(FilePath);
                var games = JsonConvert.DeserializeObject<List<Game>>(jsonData) ?? new List<Game>();
                return games.SingleOrDefault(g => g.Id == id);
            }
            finally
            {
                _syncRoot.Release();
            }
        }

        public async Task SaveAsync(Game game)
        {
            await _syncRoot.WaitAsync();
            try
            {
                var jsonData = File.Exists(FilePath) ? await File.ReadAllTextAsync(FilePath) : "";
                var games = JsonConvert.DeserializeObject<List<Game>>(jsonData) ?? new List<Game>();

                var existingGame = games.SingleOrDefault(g => g.Id == game.Id);
                if (existingGame != null)
                {
                    games.Remove(existingGame);
                }

                games.Add(game);

                jsonData = JsonConvert.SerializeObject(games);
                await File.WriteAllTextAsync(FilePath, jsonData);
            }
            finally
            {
                _syncRoot.Release();
            }
        }

        public async Task UpdateAsync(Game game)
        {
            await SaveAsync(game);
        }

        public async Task DeleteAsync(Guid gameId)
        {
            await _syncRoot.WaitAsync();
            try
            {
                var jsonData = File.Exists(FilePath) ? await File.ReadAllTextAsync(FilePath) : "";
                var games = JsonConvert.DeserializeObject<List<Game>>(jsonData) ?? new List<Game>();

                var gameToDelete = games.SingleOrDefault(g => g.Id == gameId);
                if (gameToDelete != null)
                {
                    games.Remove(gameToDelete);

                    jsonData = JsonConvert.SerializeObject(games);
                    await File.WriteAllTextAsync(FilePath, jsonData);
                }
            }
            finally
            {
                _syncRoot.Release();
            }
        }
    }
}

