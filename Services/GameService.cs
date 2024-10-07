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

        // Method to create a new game table
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

        // Method to start the game (Add this method)
        public async Task<bool> StartGame(Guid gameTableId)
        {
            var gameTable = await _context.GameTables.FindAsync(gameTableId);
            if (gameTable == null) return false;

            gameTable.GameStatus = "Started";
            await _context.SaveChangesAsync();
            return true;
        }

        // Method to end the current round (Add this method)
        public async Task<bool> EndRound(Guid gameTableId)
        {
            var gameTable = await _context.GameTables.FindAsync(gameTableId);
            if (gameTable == null) return false;

            gameTable.CurrentRound += 1;
            await _context.SaveChangesAsync();
            return true;
        }

        // Method to get the status of the game (Add this method)
        public async Task<GameTable?> GetGameStatus(Guid gameTableId)
        {
            return await _context.GameTables
                                 .Include(gt => gt.Players)
                                 .Include(gt => gt.WordList)
                                 .FirstOrDefaultAsync(gt => gt.GameTableId == gameTableId);
        }

        // Method to generate a random table key
        private string GenerateRandomTableKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 7).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
