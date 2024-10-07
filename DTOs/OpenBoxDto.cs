using System;
using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class OpenBoxDto
    {
        [Required]
        public Guid GameTableId { get; set; }

        [Required]
        public Guid PlayerId { get; set; }
    }
}
