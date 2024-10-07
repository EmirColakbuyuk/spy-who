using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.DTOs;
using SpyFallBackend.Models;
using SpyFallBackend.Services;
using System;
using System.Threading.Tasks;

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create a new word list using the service
                var wordList = await _wordListService.CreateWordList(wordListCreateDto.GameTableId, wordListCreateDto.Name, wordListCreateDto.Words);
                return Ok(wordList); // Return the created word list object
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Return bad request with the exception message if GameTableId is invalid
            }
        }

        // GET: api/WordList/Get?wordListId={wordListId}
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
