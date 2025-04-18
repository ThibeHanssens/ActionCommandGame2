using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;

namespace ActionCommandGame.Sdk
{
    public class IdentitySdk : IIdentityService<AuthenticationResult>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public IdentitySdk(
            IHttpClientFactory httpClientFactory,
            ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<AuthenticationResult> SignIn(UserSignInRequest request)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            var resp = await client.PostAsJsonAsync("identity/sign-in", request);
            resp.EnsureSuccessStatusCode();

            var result = await resp.Content
                                   .ReadFromJsonAsync<AuthenticationResult>()
                         ?? new AuthenticationResult();

            if (result.Token is not null)
                await _tokenStore.SaveTokenAsync(result.Token);

            return result;
        }

        public async Task<AuthenticationResult> Register(UserRegistrationRequest request)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            var resp = await client.PostAsJsonAsync("identity/register", request);
            resp.EnsureSuccessStatusCode();

            var result = await resp.Content
                                   .ReadFromJsonAsync<AuthenticationResult>()
                         ?? new AuthenticationResult();

            if (result.Token is not null)
                await _tokenStore.SaveTokenAsync(result.Token);

            return result;
        }
    }
}
