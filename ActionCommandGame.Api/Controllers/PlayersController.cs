using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PlayersController : ApiBaseController
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerService.Get(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Find([FromQuery]PlayerFilter filter)
        {
            var result = await _playerService.Find(filter);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlayerCreateRequest req)
        {
            var result = await _playerService.Create(req);
            return Ok(result);
        }
    }
}
