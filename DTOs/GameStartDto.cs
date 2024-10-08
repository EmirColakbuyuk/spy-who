using System;
using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class GameStartDto
    {
        [Required]
        public Guid GameTableId { get; set; }

        [Required]
        public Guid WordListId { get; set; }
    }
}
