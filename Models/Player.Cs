using System.Text.Json.Serialization;

namespace SpyFallBackend.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsSpy { get; set; } = false;  // Is the player a spy
        public int GameSessionId { get; set; }  // Foreign key to the GameSession

        [JsonIgnore]  // Prevent cyclic reference by ignoring this during serialization
        public GameSession GameSession { get; set; }

        public bool IsRevealed { get; set; } = false;  // Whether the player has revealed their role
    }
}
