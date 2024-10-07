using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class WordListCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MinLength(1, ErrorMessage = "You must provide at least one word.")]
        public List<string> Words { get; set; } = new List<string>();

        // Include GameTableId to associate the WordList with an existing GameTable
        [Required]
        public Guid GameTableId { get; set; }
    }
}
