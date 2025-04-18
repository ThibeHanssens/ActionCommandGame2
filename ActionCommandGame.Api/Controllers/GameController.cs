using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class GameController : ApiBaseController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("game/{playerId}/perform-action")]
        public async Task<IActionResult> PerformAction(int playerId)
        {
            var result = await _gameService.PerformAction(playerId);
            return Ok(result);
        }

        [HttpPost("game/{playerId}/buy/{itemId}")]
        public async Task<IActionResult> Buy(int playerId, int itemId)
        {
            var result = await _gameService.Buy(playerId, itemId);
            return Ok(result);
        }
    }
}
