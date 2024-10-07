using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SpyFallBackend.Models
{
    public class Word
    {
        [Key]
        public Guid WordId { get; set; } = Guid.NewGuid();

        [ForeignKey("WordList")]
        public Guid WordListId { get; set; }
        public virtual WordList? WordList { get; set; }

        [Required]
        [MaxLength(50)]
        public string? WordText { get; set; } // The word itself
    }
}
