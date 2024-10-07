using SpyFallBackend.Data;
using SpyFallBackend.Models;

namespace SpyFallBackend.Services
{
    public class PlayerService
    {
        private readonly ApplicationDbContext _context;

        public PlayerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Player> AddPlayer(Guid gameTableId, string playerName)
        {
            var player = new Player
            {
                GameTableId = gameTableId,
                PlayerName = playerName
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();
            return player;
        }

        public async Task<Player?> GetPlayerStatus(Guid playerId)
        {
            // Use FindAsync and return null if not found to handle nullable return
            var player = await _context.Players.FindAsync(playerId);
            return player; // Will return null if player is not found
        }
    }
}
