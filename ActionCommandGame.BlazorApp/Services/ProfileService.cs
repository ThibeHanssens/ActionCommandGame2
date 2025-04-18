using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Sdk;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.BlazorApp.Services
{
    public class ProfileService
    {
        private readonly ProfileSdk _sdk;

        public ProfileService(ProfileSdk sdk)
        {
            _sdk = sdk;
        }

        public async Task<AuthenticationResult> UpdateProfileAsync(UserProfileUpdateRequest request)
        {
            return await _sdk.UpdateProfileAsync(request);
        }

        public async Task<UserProfileResult?> GetProfileAsync()
        {
            return await _sdk.GetProfileAsync();
        }
    }
}
