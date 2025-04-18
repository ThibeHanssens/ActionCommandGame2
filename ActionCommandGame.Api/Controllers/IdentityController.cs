
using System.Security.Claims;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService<AuthenticationResult> _identityService;

        public IdentityController(IIdentityService<AuthenticationResult> identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("identity/sign-in")]
        public async Task<IActionResult> SignIn(UserSignInRequest request)
        {
            var authenticationResult = await _identityService.SignIn(request);
            return Ok(authenticationResult);
        }

        [HttpPost("identity/register")]
        public async Task<IActionResult> Register(UserRegistrationRequest request)
        {
            var authenticationResult = await _identityService.Register(request);
            return Ok(authenticationResult);
        }
    }
}
