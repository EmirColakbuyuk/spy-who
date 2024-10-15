using Microsoft.EntityFrameworkCore;
using SpyFallBackend.Models;

namespace SpyFallBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet properties to manage each entity
        public DbSet<GameTable> GameTables { get; set; }
        public DbSet<WordList> WordLists { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Player> Players { get; set; }

        // Configure the model relationships and table names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure GameTable relationships
            modelBuilder.Entity<GameTable>()
                .HasOne(gt => gt.WordList)
                .WithMany() // No reverse navigation from WordList to GameTable
                .HasForeignKey(gt => gt.WordListId); // GameTable has a nullable WordListId

            modelBuilder.Entity<GameTable>()
                .HasMany(gt => gt.Players)
                .WithOne(p => p.GameTable)
                .HasForeignKey(p => p.GameTableId);

            // Configure WordList relationships
            modelBuilder.Entity<WordList>()
                .HasMany(wl => wl.Words)
                .WithOne(w => w.WordList)
                .HasForeignKey(w => w.WordListId);
        }
    }
}
