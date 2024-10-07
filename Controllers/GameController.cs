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
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        // 1. Create game without word list
        [HttpPost("Create")]
        public async Task<ActionResult<GameTable>> CreateGame([FromBody] GameCreateDto gameCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (gameCreateDto.PlayerCount < 2 || gameCreateDto.PlayerCount > 10)
            {
                return BadRequest("Player count must be between 2 and 10.");
            }

            var gameTable = await _gameService.CreateGameTable(gameCreateDto.PlayerCount);
            return Ok(gameTable);
        }

        // 2. Start game with word list ID
        [HttpPost("Start")]
        public async Task<ActionResult> StartGame([FromBody] GameStartDto gameStartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _gameService.StartGame(gameStartDto.GameTableId, gameStartDto.WordListId);
            if (!result)
            {
                return BadRequest("Failed to start the game. Make sure the game table ID and word list ID are correct.");
            }
            
            // You can return the full game table status or a more detailed response as needed
            var gameStatus = await _gameService.GetGameStatus(gameStartDto.GameTableId);
            return Ok(new { Message = "Game started successfully.", GameStatus = gameStatus });
        }

        // 3. End round and move to next
        [HttpPost("EndRound")]
        public async Task<ActionResult> EndRound([FromBody] GameRoundDto gameRoundDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _gameService.EndRound(gameRoundDto.GameTableId);
            if (!result)
            {
                return BadRequest("Failed to end the round. Make sure the game table ID is correct.");
            }

            // Include updated game status or round details in the response if necessary
            var gameStatus = await _gameService.GetGameStatus(gameRoundDto.GameTableId);
            return Ok(new { Message = "Round ended successfully.", GameStatus = gameStatus });
        }

        // 4. Get game status
        [HttpGet("GetGameStatus")]
        public async Task<ActionResult<GameTable>> GetGameStatus([FromQuery] Guid gameTableId)
        {
            var gameStatus = await _gameService.GetGameStatus(gameTableId);
            if (gameStatus == null)
            {
                return NotFound("Game table not found.");
            }
            return Ok(gameStatus);
        }
    

        [HttpPost("OpenBox")]
        public async Task<ActionResult> OpenBox([FromBody] OpenBoxDto openBoxDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _gameService.OpenBox(openBoxDto.GameTableId, openBoxDto.PlayerId);
            if (response == null)
            {
                return BadRequest("Failed to open box. Make sure the game table ID and player ID are correct.");
            }
            return Ok(response);
        }
    }
}
