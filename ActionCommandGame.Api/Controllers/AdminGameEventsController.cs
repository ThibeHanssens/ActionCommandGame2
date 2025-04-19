using ActionCommandGame.Api;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("game-events")]
[Authorize(Roles = "SuperAdmin")]
public class AdminGameEventsController : ApiBaseController
{
    private readonly IPositiveGameEventService _posSvc;
    private readonly INegativeGameEventService _negSvc;

    public AdminGameEventsController(
        IPositiveGameEventService posSvc,
        INegativeGameEventService negSvc)
    {
        _posSvc = posSvc;
        _negSvc = negSvc;
    }

    [HttpGet("positive")]
    public async Task<IActionResult> PositiveFind()
        => Ok(await _posSvc.Find());

    [HttpPost("positive")]
    public async Task<IActionResult> PositiveCreate(
        [FromBody] PositiveGameEventCreateRequest req)
        => Ok(await _posSvc.Create(req));

    [HttpDelete("positive/{id}")]
    public async Task<IActionResult> PositiveDelete(int id)
        => Ok(await _posSvc.Delete(id));

    [HttpGet("negative")]
    public async Task<IActionResult> NegativeFind()
        => Ok(await _negSvc.Find());

    [HttpPost("negative")]
    public async Task<IActionResult> NegativeCreate(
        [FromBody] NegativeGameEventCreateRequest req)
        => Ok(await _negSvc.Create(req));

    [HttpDelete("negative/{id}")]
    public async Task<IActionResult> NegativeDelete(int id)
        => Ok(await _negSvc.Delete(id));
}
