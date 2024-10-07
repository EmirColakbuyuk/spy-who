using SpyFallBackend.Data;
using SpyFallBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace SpyFallBackend.Services
{
    public class WordListService
    {
        private readonly ApplicationDbContext _context;

        public WordListService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to create a new word list with the specified name and list of words.
        public async Task<WordList> CreateWordList(Guid gameTableId, string name, List<string> words)
        {
            // Check if the GameTableId exists in the database before proceeding
            var gameTableExists = await _context.GameTables.AnyAsync(gt => gt.GameTableId == gameTableId);
            if (!gameTableExists)
            {
                throw new InvalidOperationException("The specified GameTableId does not exist.");
            }

            // Create the new word list
            var wordList = new WordList
            {
                GameTableId = gameTableId, // Associate with the existing GameTable
                Name = name,
                Words = words.Select(word => new Word { WordText = word }).ToList()
            };

            _context.WordLists.Add(wordList);
            await _context.SaveChangesAsync();

            return wordList;
        }


        // Method to retrieve a word list by its ID.
        public async Task<WordList?> GetWordList(Guid wordListId)
        {
            return await _context.WordLists
                                 .Include(wl => wl.Words)
                                 .FirstOrDefaultAsync(wl => wl.WordListId == wordListId);
        }
    }
}
