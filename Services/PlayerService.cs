using SpyFallBackend.Data;
using SpyFallBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace SpyFallBackend.Services
{
    public class PlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to add a player to the specified game table.
        public async Task<Player?> AddPlayer(Guid gameTableId, string playerName)
        {
            // Check if the game table exists.
            var gameTable = await _context.GameTables.FindAsync(gameTableId);
            if (gameTable == null)
            {
                return null;
            }

            // Create a new player instance with the provided player name.
            var player = new Player
            {
                GameTableId = gameTableId,
                PlayerName = playerName,
                IsSpy = false, // Default value for new players
                BoxOpened = false
            };

            // Add the player to the context and save changes.
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return player;
        }

        // Method to get the player's status by their ID.
        public async Task<Player?> GetPlayerStatus(Guid playerId)
        {
            return await _context.Players.FindAsync(playerId);
        }
    }
}
