using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.DTOs; // Import the DTOs namespace
using SpyFallBackend.Models;
using SpyFallBackend.Services;

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

        // POST: api/Game/Create
        [HttpPost("Create")]
        public async Task<ActionResult<GameTable>> CreateGame([FromBody] GameCreateDto gameCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gameTable = await _gameService.CreateGameTable(gameCreateDto.PlayerCount);
            return Ok(gameTable);
        }

        // POST: api/Game/Start
        [HttpPost("Start")]
        public async Task<ActionResult> StartGame([FromBody] GameStartDto gameStartDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _gameService.StartGame(gameStartDto.GameTableId);
            if (!result)
            {
                return BadRequest("Failed to start the game. Make sure the game table ID is correct.");
            }
            return Ok("Game started successfully.");
        }

        // POST: api/Game/EndRound
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
            return Ok("Round ended successfully.");
        }

        // GET: api/Game/GetGameStatus
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
    }
}
