
using System.Security.Claims;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }
        
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _profileService.GetProfileAsync();
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPost("profile/update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateRequest request)
        {
            var result = await _profileService.UpdateProfileAsync(request);
            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok(result);
        }
    }
}
