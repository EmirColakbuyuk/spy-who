using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<GameTable>> CreateGame(int playerCount)
        {
            var gameTable = await _gameService.CreateGameTable(playerCount);
            return Ok(gameTable);
        }

        // POST: api/Game/Start
        [HttpPost("Start")]
        public async Task<ActionResult> StartGame([FromQuery] Guid gameTableId)
        {
            var result = await _gameService.StartGame(gameTableId);
            if (!result)
            {
                return BadRequest("Failed to start the game. Make sure the game table ID is correct.");
            }
            return Ok("Game started successfully.");
        }

        // POST: api/Game/EndRound
        [HttpPost("EndRound")]
        public async Task<ActionResult> EndRound([FromQuery] Guid gameTableId)
        {
            var result = await _gameService.EndRound(gameTableId);
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
