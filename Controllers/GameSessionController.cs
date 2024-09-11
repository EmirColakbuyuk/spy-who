using Microsoft.AspNetCore.Mvc;
using SpyFallBackend.Services;
using System.Collections.Generic;


namespace SpyFallBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        // Create a new game with players
        [HttpPost("create")]
        public IActionResult CreateGame([FromBody] CreateGameRequest request)
        {
            try
            {
                var gameSession = _gameService.CreateGame(request.PlayerNames, request.Word);
                return Ok(gameSession);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Reveal a player's role
        [HttpPost("reveal")]
        public IActionResult RevealRole([FromBody] RevealRoleRequest request)
        {
            try
            {
                var result = _gameService.RevealRole(request.GameId, request.PlayerId);
                return Ok(new { message = result });
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class CreateGameRequest
    {
        public List<string>? PlayerNames { get; set; }
        public string? Word { get; set; }
    }

    public class RevealRoleRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
    }
}
