using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class GameCreateDto
    {
        [Required]
        [Range(3, 12, ErrorMessage = "Player count must be between 3 and 12.")]
        public int PlayerCount { get; set; }
        public Guid WordListId { get; set; } // Ensure WordListId is present in the DTO
    
    }
}
