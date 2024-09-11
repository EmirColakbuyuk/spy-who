using Microsoft.EntityFrameworkCore;
using SpyFallBackend.Models;

namespace SpyFallBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Player> Players { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameSession>()
                .HasMany(gs => gs.Players)
                .WithOne(p => p.GameSession)
                .HasForeignKey(p => p.GameSessionId)
                .OnDelete(DeleteBehavior.Cascade);  // Delete all players when session is deleted
        }
    }
}
