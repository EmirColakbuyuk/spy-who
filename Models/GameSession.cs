using System;
using System.Collections.Generic;

namespace SpyFallBackend.Models
{
    public class GameSession
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Player> Players { get; set; } = new List<Player>();  // Players in the session
        public string Word { get; set; } = string.Empty;  // Word for non-spy players
        public bool IsActive { get; set; } = true;  // If the game session is still active
    }
}
