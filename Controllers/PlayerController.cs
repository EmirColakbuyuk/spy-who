using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.Models;
using SpyFallBackend.Services;

namespace SpyFallBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly PlayerService _playerService;

        public PlayerController(PlayerService playerService)
        {
            _playerService = playerService;
        }

        // POST: api/Player/AddPlayer
        [HttpPost("AddPlayer")]
        public async Task<ActionResult<Player>> AddPlayer([FromQuery] Guid gameTableId, [FromBody] string playerName)
        {
            var player = await _playerService.AddPlayer(gameTableId, playerName);
            return Ok(player);
        }

        // GET: api/Player/GetPlayerStatus
        [HttpGet("GetPlayerStatus")]
        public async Task<ActionResult<Player>> GetPlayerStatus([FromQuery] Guid playerId)
        {
            var player = await _playerService.GetPlayerStatus(playerId);
            if (player == null)
            {
                return NotFound("Player not found.");
            }
            return Ok(player);
        }
    }
}
