using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class PlayerItemsController : ApiBaseController
    {
        private readonly IPlayerItemService _playerItemService;

        public PlayerItemsController(IPlayerItemService playerItemService)
        {
            _playerItemService = playerItemService;
        }

        [HttpGet("player-items")]
        public async Task<IActionResult> Find([FromQuery] PlayerItemFilter filter)
        {
            var result = await _playerItemService.Find(filter);
            return Ok(result);
        }

        [HttpDelete("player-items/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _playerItemService.Delete(id);
            return Ok(result);
        }
    }
}
