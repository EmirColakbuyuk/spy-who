using System;
using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class GameRoundDto
    {
        [Required]
        public Guid GameTableId { get; set; }
    }
}
