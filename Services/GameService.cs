using System;
using System.Collections.Generic;
using System.Linq;
using SpyFallBackend.Models;
using SpyFallBackend.Data;

namespace SpyFallBackend.Services
{
    public class GameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new game session with players
        public GameSession CreateGame(List<string> playerNames, string word)
        {
            if (playerNames.Count < 3)
            {
                throw new Exception("At least 3 players are required.");
            }

            // Create the player objects
            var players = playerNames.Select(name => new Player { Name = name }).ToList();

            // Randomly assign one player to be the spy
            var random = new Random();
            var spyIndex = random.Next(players.Count);
            players[spyIndex].IsSpy = true;

            // Create the game session
            var gameSession = new GameSession
            {
                Players = players,
                Word = word,
                IsActive = true
            };

            _context.GameSessions.Add(gameSession);
            _context.SaveChanges();

            return gameSession;
        }

        // Reveal the player's role (spy or word)
        public string RevealRole(int gameId, int playerId)
        {
            var gameSession = _context.GameSessions
                .FirstOrDefault(gs => gs.Id == gameId);

            if (gameSession == null)
            {
                throw new Exception("Game session not found.");
            }

            var player = gameSession.Players.FirstOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                throw new Exception("Player not found.");
            }

            if (player.IsRevealed)
            {
                throw new Exception("Player has already revealed their role.");
            }

            // Mark the player as having revealed their role
            player.IsRevealed = true;
            _context.SaveChanges();

            return player.IsSpy ? "You are the spy!" : $"Your word is: {gameSession.Word}";
        }
    }
}
