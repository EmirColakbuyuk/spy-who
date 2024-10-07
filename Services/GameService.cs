using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SpyFallBackend.Data;
using SpyFallBackend.Models;

namespace SpyFallBackend.Services
{
    public class GameService
    {
        private readonly ApplicationDbContext _context;

        public GameService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GameTable> CreateGameTable(int playerCount)
        {
            var gameTable = new GameTable
            {
                PlayerCount = playerCount,
                TableKey = GenerateRandomTableKey()
            };

            _context.GameTables.Add(gameTable);
            await _context.SaveChangesAsync();
            return gameTable;
        }

        // Method to start the game and automatically assign a spy
        public async Task<bool> StartGame(Guid gameTableId)
        {
            // Find the game table by ID
            var gameTable = await _context.GameTables
                    .Include(gt => gt.Players) // Include players when retrieving the game table
                    .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);
            
            if (gameTable == null) return false;

            // Check if the game has already been started
            if (gameTable.GameStatus == "Started")
            {
                throw new InvalidOperationException("The game has already started.");
            }

            // Set the game status to "Started"
            gameTable.GameStatus = "Started";

            // Automatically assign a random player as the spy
            var spyAssigned = await AssignRandomSpy(gameTableId);
            if (!spyAssigned)
            {
                throw new InvalidOperationException("Failed to assign a spy. Make sure there are players in the game.");
            }

            // Save the changes to the game table and players
            await _context.SaveChangesAsync();
            return true;
        }

        // Method to randomly assign a player as the spy
        private async Task<bool> AssignRandomSpy(Guid gameTableId)
        {
            // Retrieve the game table along with its players
            var gameTable = await _context.GameTables.Include(gt => gt.Players).FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);

            // If no game table is found or there are no players, return false
            if (gameTable == null || !gameTable.Players.Any())
            {
                return false;
            }

            Console.WriteLine($"Total players in the game table: {gameTable.Players.Count}");

            // Reset all players to not be spies
            foreach (var player in gameTable.Players)
            {
                player.IsSpy = false;
            }

            // Randomly select a player to be the spy
            var random = new Random();
            int spyIndex = random.Next(gameTable.Players.Count);
            var selectedSpy = gameTable.Players.ElementAt(spyIndex);
            selectedSpy.IsSpy = true;

            

            // Save the changes to the database
            await _context.SaveChangesAsync();



            var verifySpy = await _context.Players.Where(p => p.GameTableId == gameTableId && p.IsSpy == true).FirstOrDefaultAsync();

            if (verifySpy != null)
            {
                Console.WriteLine($"Successfully verified player {verifySpy.PlayerName} as the spy.");
                return true;
            }

            Console.WriteLine("Failed to set a player as the spy.");
            return false;

        }
        public async Task<bool> EndRound(Guid gameTableId)
        {
            var gameTable = await _context.GameTables.FindAsync(gameTableId);
            if (gameTable == null) return false;

            gameTable.CurrentRound += 1;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<GameTable?> GetGameStatus(Guid gameTableId)
        {
            return await _context.GameTables
                                 .Include(gt => gt.Players)
                                 .Include(gt => gt.WordList)
                                 .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);
        }

        private string GenerateRandomTableKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
