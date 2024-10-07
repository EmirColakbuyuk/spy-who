using Microsoft.EntityFrameworkCore;
using SpyFallBackend.Data;
using SpyFallBackend.Models;

namespace SpyFallBackend.Services
{
    public class WordListService
    {
        private readonly ApplicationDbContext _context;

        public WordListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WordList> CreateWordList(List<string> words)
        {
            // Create a new WordList and initialize the Words collection
            var wordList = new WordList
            {
                Words = words.Select(w => new Word { WordText = w }).ToList()
            };

            _context.WordLists.Add(wordList);
            await _context.SaveChangesAsync();
            return wordList;
        }

        public async Task<WordList?> GetWordList(Guid wordListId)
        {
            // Use FirstOrDefaultAsync to handle potential null results
            return await _context.WordLists
                                 .Include(wl => wl.Words)
                                 .FirstOrDefaultAsync(wl => wl.WordListId == wordListId);
        }
    }
}
