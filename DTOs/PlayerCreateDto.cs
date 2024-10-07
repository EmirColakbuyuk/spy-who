using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class PlayerCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string PlayerName { get; set; } = string.Empty;
    }
}
