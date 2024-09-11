using System;

namespace SpyFallBackend.Models
{
    public class Round
    {
        public int Id { get; set; }
        public int GameSessionId { get; set; }
        public string Word { get; set; } = string.Empty; 
        public int SpyPlayerId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
