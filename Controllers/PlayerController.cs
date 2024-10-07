using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.DTOs; // Make sure to import the DTOs namespace
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
        public async Task<IActionResult> AddPlayer([FromQuery] Guid gameTableId, [FromBody] PlayerCreateDto player)
        {
            // Check if the request body is valid according to the DTO validation attributes.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call the service to add the player and pass the player name from the DTO.
            var result = await _playerService.AddPlayer(gameTableId, player.PlayerName);
            if (result == null)
            {
                // Return a bad request if adding the player fails, possibly due to an invalid gameTableId.
                return BadRequest("Failed to add player. Please check the gameTableId.");
            }

            // Return the newly created player information.
            return Ok(result);
        }

        // GET: api/Player/GetPlayerStatus
        [HttpGet("GetPlayerStatus")]
        public async Task<ActionResult<Player>> GetPlayerStatus([FromQuery] Guid playerId)
        {
            // Retrieve the player's status from the service using the playerId.
            var player = await _playerService.GetPlayerStatus(playerId);
            if (player == null)
            {
                // Return 404 if the player is not found.
                return NotFound("Player not found.");
            }

            // Return the player information.
            return Ok(player);
        }
    }
}
