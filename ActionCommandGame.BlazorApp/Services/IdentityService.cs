using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Sdk;

namespace ActionCommandGame.BlazorApp.Services
{
    public class IdentityService
    {
        private readonly IdentityApi _sdk;

        public IdentityService(IdentityApi sdk)
        {
            _sdk = sdk;
        }

        public async Task<AuthenticationResult> SignInAsync(UserSignInRequest request)
        {
            return await _sdk.SignIn(request);
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            return await _sdk.Register(request);
        }
    }
}
