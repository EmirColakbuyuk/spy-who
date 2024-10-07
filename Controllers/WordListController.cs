using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.Models;
using SpyFallBackend.Services;

namespace SpyFallBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordListController : ControllerBase
    {
        private readonly WordListService _wordListService;

        public WordListController(WordListService wordListService)
        {
            _wordListService = wordListService;
        }

        // POST: api/WordList/Create
        [HttpPost("Create")]
        public async Task<ActionResult<WordList>> CreateWordList([FromBody] List<string> words)
        {
            var wordList = await _wordListService.CreateWordList(words);
            return Ok(wordList);
        }

        // GET: api/WordList/Get
        [HttpGet("Get")]
        public async Task<ActionResult<WordList>> GetWordList([FromQuery] Guid wordListId)
        {
            var wordList = await _wordListService.GetWordList(wordListId);
            if (wordList == null)
            {
                return NotFound("Word list not found.");
            }
            return Ok(wordList);
        }
    }
}
