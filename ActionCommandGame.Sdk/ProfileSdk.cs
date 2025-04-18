using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Results;
using System.Net;

namespace ActionCommandGame.Sdk
{
    public class ProfileSdk : IProfileService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public ProfileSdk(
            IHttpClientFactory httpClientFactory,
            ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<UserProfileResult?> GetProfileAsync()
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            client.AddAuthorization(token);

            return await client.GetFromJsonAsync<UserProfileResult>("profile");
        }

        public async Task<AuthenticationResult> UpdateProfileAsync(UserProfileUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            client.AddAuthorization(token);

            var resp = await client.PostAsJsonAsync("profile/update-profile", request);

            if (resp.StatusCode == HttpStatusCode.BadRequest)
            {
                var errors = await resp.Content
                                       .ReadFromJsonAsync<string[]>()
                                   ?? Array.Empty<string>();
                return new AuthenticationResult { Errors = errors };
            }
            resp.EnsureSuccessStatusCode();
            return await resp.Content
                             .ReadFromJsonAsync<AuthenticationResult>()
                   ?? new AuthenticationResult();
        }
    }
}
