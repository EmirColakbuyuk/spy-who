using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.DTOs; // Import the DTOs namespace
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
        public async Task<ActionResult<WordList>> CreateWordList([FromBody] WordListCreateDto wordListCreateDto)
        {
            // Validate the incoming request using ModelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new word list using the service and the values from the DTO
            var wordList = await _wordListService.CreateWordList(wordListCreateDto.Name, wordListCreateDto.Words);
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
