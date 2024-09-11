using System.Collections.Generic;

namespace SpyFallBackend.Models
{
    public class WordList
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public ICollection<string> Words { get; set; } = new List<string>(); 
    }
}
