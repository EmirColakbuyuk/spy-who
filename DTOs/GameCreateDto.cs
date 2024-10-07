using System.ComponentModel.DataAnnotations;

namespace SpyFallBackend.DTOs
{
    public class GameCreateDto
    {
        [Required]
        [Range(2, 10, ErrorMessage = "Player count must be between 2 and 10.")]
        public int PlayerCount { get; set; }
        public Guid WordListId { get; set; } // Ensure WordListId is present in the DTO
    
    }
}
