using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TestAPI.Models;
using TestAPI.Repository;

namespace TestAPI.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<Game> CreateGameAsync(string userName1)
        {
            var player1 = new Player { Id = Guid.NewGuid(), Name = userName1 };
            var game = new Game(player1);

            await _gameRepository.SaveAsync(game);

            return game;
        }

        public async Task<Guid> JoinGameAsync(Guid gameId, string userName2)
        {
            var game = await _gameRepository.GetAsync(gameId);

            if (game == null)
            {
                throw new Exception("Game not found");
            }

            if (game.Player2 != null)
            {
                throw new Exception("Game already has two players");
            }

            var player2 = new Player { Id = Guid.NewGuid(), Name = userName2 };
            game.Player2 = player2;

            await _gameRepository.SaveAsync(game);

            return player2.Id;
        }

        public async Task<TurnResult> MakeTurnAsync(Guid gameId, Guid userId, Turn turn)
        {
            var game = await _gameRepository.GetAsync(gameId);

            // Проверка на существование игры 
            if (game == null)
            {
                throw new Exception("Game is not exist");
            }
            // Проверка на то, закончена ли игра
            if (game.IsOver)
            {
                throw new Exception("Game is already over");
            }

            var round = game.CurrentRound ?? new Round();
            // Проверка на то, есть ли такой игрок, сделал ли он ход
            if (game.Player1.Id == userId)
            {
                if (round.Player1Turn != null)
                {
                    throw new Exception("Player 1 has already made a turn");
                }
                round.Player1Turn = turn;
            }
            else if (game.Player2.Id == userId)
            {
                if (round.Player2Turn != null)
                {
                    throw new Exception("Player 2 has already made a turn");
                }
                round.Player2Turn = turn;
            }
            else
            {
                throw new Exception("Player not found in this game");
            }

            // Если ходы сделаны, то смотрим результаты раунда,
            // Если раундов 5, то подводим итог игры
            if (round.Player1Turn != null && round.Player2Turn != null)
            {
                round.Result = CalculateRoundResult(round);
                game.Rounds.Add(round);
                game.CurrentRound = null;
                if (game.Rounds.Count >= 5)
                {
                    game.Abort();
                    game.Result = CalculateGameResult(game); 
                }
            }
            else
            {
                game.CurrentRound = round;
            }

            await _gameRepository.UpdateAsync(game);

            return new TurnResult
            {
                IsGameOver = game.IsOver,
                RoundResult = round.Result,
                GameResult = game.IsOver ? CalculateGameResult(game) : null
            };
        }

        public async Task CancelGameAsync(Guid gameId)
        {
            var game = await _gameRepository.GetAsync(gameId);

            if (game == null)
            {
                throw new Exception("Game not found");
            }

            game.Abort();
            game.Status = GameStatus.Cancelled;

            await _gameRepository.UpdateAsync(game);
        }

        private RoundResult CalculateRoundResult(Round round)
        {
            if (round.Player1Turn == round.Player2Turn)
            {
                return RoundResult.Draw;
            }

            if (round.Player1Turn == Turn.Rock && round.Player2Turn == Turn.Scissors)
            {
                return RoundResult.Player1Wins;
            }

            if (round.Player1Turn == Turn.Scissors && round.Player2Turn == Turn.Paper)
            {
                return RoundResult.Player1Wins;
            }

            if (round.Player1Turn == Turn.Paper && round.Player2Turn == Turn.Rock)
            {
                return RoundResult.Player1Wins;
            }

            return RoundResult.Player2Wins;
        }

        private GameResult CalculateGameResult(Game game)
        {
            int player1Wins = game.Rounds.Count(r => r.Result == RoundResult.Player1Wins);
            int player2Wins = game.Rounds.Count(r => r.Result == RoundResult.Player2Wins);

            if (player1Wins > player2Wins)
            {
                return GameResult.Player1Wins;
            }

            if (player2Wins > player1Wins)
            {
                return GameResult.Player2Wins;
            }

            return GameResult.Draw;
        }

        public async Task<GameStats> GetGameStatsAsync(Guid gameId)
        {
            var game = await _gameRepository.GetAsync(gameId);

            if (game == null)
            {
                throw new Exception("Game not found");
            }

            var player1Wins = game.Rounds.Count(r => r.Result == RoundResult.Player1Wins);
            var player2Wins = game.Rounds.Count(r => r.Result == RoundResult.Player2Wins);
            var draws = game.Rounds.Count(r => r.Result == RoundResult.Draw);

            var gameStats = new GameStats
            {
                Player1Wins = player1Wins,
                Player2Wins = player2Wins,
                Draws = draws
            };

            game.GameStats = gameStats;
            await _gameRepository.UpdateAsync(game);

            return gameStats;
        }
    }
}

