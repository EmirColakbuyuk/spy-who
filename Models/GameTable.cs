using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.Models
{
    public class GameTable
    {
        [Key]
        public Guid GameTableId { get; set; } = Guid.NewGuid(); //Guid is used to generate key for GameTable that is globally unique

        [Required]
        [MaxLength(10)]
        public string? TableKey { get; set; } 

        [Required]
        public int PlayerCount { get; set; }

        public int CurrentRound { get; set; } = 1;

        [Required]
        [MaxLength(20)]
        public string GameStatus { get; set; } = "Ongoing"; 

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        
        public virtual ICollection<Player> Players { get; set; } = new List<Player>();
        public virtual WordList? WordList { get; set; }
    }
}
