using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyFallBackend.Models
{
    public class WordList
    {
        [Key]
        public Guid WordListId { get; set; } = Guid.NewGuid();

        [ForeignKey("GameTable")]
        public Guid GameTableId { get; set; }
        public virtual GameTable? GameTable { get; set; }

        [MaxLength(100)]
        public string? Name { get; set; } 

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Word>? Words { get; set; }
    }
}
