using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Requests;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Route("admin/items")]
    [Authorize(Roles = "SuperAdmin")]
    public class AdminItemsController : ApiBaseController
    {
        private readonly IItemService _itemService;

        public AdminItemsController(IItemService itemService,
                                    ActionCommandGameDbContext dbContext)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Find()
            => Ok(await _itemService.Find());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
            => Ok(await _itemService.Get(id));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemCreateRequest request)
            => Ok(await _itemService.Create(request));

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ItemUpdateRequest req)
        {
            req.Id = id;
            return Ok(await _itemService.Update(req));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
            => Ok(await _itemService.Delete(id));
    }
}
