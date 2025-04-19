using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemSdk : IPlayerItemService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public PlayerItemSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<IList<PlayerItemResult>>> FindAsync(PlayerItemFilter filter)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);

            var route = "player-items";
            if (filter.PlayerId.HasValue)
            {
                route += $"?playerId={filter.PlayerId}";
            }

            var httpResponse = await httpClient.GetAsync(route);
            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content
                .ReadFromJsonAsync<ServiceResult<IList<PlayerItemResult>>>();

            return result ?? new ServiceResult<IList<PlayerItemResult>>();
        }

        // alias for interface compatibility
        public Task<ServiceResult<IList<PlayerItemResult>>> Find(PlayerItemFilter filter)
            => FindAsync(filter);

        public async Task<ServiceResult<PlayerItemResult>> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);

            var httpResponse = await httpClient.GetAsync($"player-items/{id}");
            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content
                .ReadFromJsonAsync<ServiceResult<PlayerItemResult>>();

            return result ?? new ServiceResult<PlayerItemResult>();
        }

        public async Task<ServiceResult<PlayerItemResult>> Create(int playerId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);

            var payload = new { playerId, itemId };
            var httpResponse = await httpClient.PostAsJsonAsync("player-items", payload);
            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content
                .ReadFromJsonAsync<ServiceResult<PlayerItemResult>>();

            return result ?? new ServiceResult<PlayerItemResult>();
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);

            var httpResponse = await httpClient.DeleteAsync($"player-items/{id}");
            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content
                .ReadFromJsonAsync<ServiceResult>();

            return result ?? new ServiceResult();
        }
    }
}
