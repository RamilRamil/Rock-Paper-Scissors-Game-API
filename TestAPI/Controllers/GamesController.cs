using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;
using TestAPI.Services;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // POST: api/games/create
        [HttpPost("create")]
        public async Task<ActionResult<Game>> CreateGame(string userName1)
        {
            var game = await _gameService.CreateGameAsync(userName1);
            return Ok(game);
        }

        // POST: api/games/{gameId}/join
        [HttpPost("{gameId}/join")]
        public async Task<ActionResult<Guid>> JoinGame(Guid gameId, [FromBody] string userName2)
        {
            var playerId = await _gameService.JoinGameAsync(gameId, userName2);
            return Ok(playerId);
        }

        // POST: api/games/{gameId}/user/{userId}/turn
        [HttpPost("{gameId}/user/{userId}/turn")]
        public async Task<ActionResult<RoundResult>> MakeTurn(Guid gameId, Guid userId, [FromBody] Turn turn)
        {
            var turnResult = await _gameService.MakeTurnAsync(gameId, userId, turn);
            return Ok(turnResult);
        }

        // GET: api/games/{gameId}/stat
        [HttpGet("{gameId}/stat")]
        public async Task<ActionResult<GameStats>> GetGameStats(Guid gameId)
        {
            var gameStats = await _gameService.GetGameStatsAsync(gameId);
            return Ok(gameStats);
        }

        // DELETE: api/games/{gameId}/cancel
        [HttpDelete("{gameId}/cancel")]
        public async Task<IActionResult> CancelGame(Guid gameId)
        {
            await _gameService.CancelGameAsync(gameId);
            return Ok();
        }
    }
}


