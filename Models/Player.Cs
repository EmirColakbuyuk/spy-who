using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SpyFallBackend.Models
{
    public class Player
    {
        [Key]
        public Guid PlayerId { get; set; } = Guid.NewGuid();

        [ForeignKey("GameTable")]
        public Guid GameTableId { get; set; }
        [JsonIgnore]
         public virtual GameTable? GameTable { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PlayerName { get; set; }

        public bool IsSpy { get; set; } = false; // Indicates if the player is the spy

        public bool BoxOpened { get; set; } = false; // Indicates if the player has opened their box
    }
}
