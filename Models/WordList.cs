using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.Models
{
    public class WordList
    {
        [Key]
        public Guid WordListId { get; set; } = Guid.NewGuid();

        [MaxLength(100)]
        public string? Name { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties (No GameTableId)
        public virtual ICollection<Word>? Words { get; set; }
    }
}
