using System.Net.Http.Json;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;

namespace ActionCommandGame.Sdk
{
    public class IdentitySdk: IIdentityService<AuthenticationResult>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentitySdk(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthenticationResult> SignIn(UserSignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var route = "identity/sign-in";

            var httpResponse = await httpClient.PostAsJsonAsync(route, request);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

            if (result is null)
            {
                return new AuthenticationResult();
            }

            return result;
        }

        public async Task<AuthenticationResult> Register(UserRegistrationRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var route = "identity/register";

            var httpResponse = await httpClient.PostAsJsonAsync(route, request);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

            if (result is null)
            {
                return new AuthenticationResult();
            }

            return result;
        }
    }
}
