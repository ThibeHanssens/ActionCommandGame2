using System.Net.Http.Json;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class GameSdk: IGameService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public GameSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<GameResult>> PerformAction(int playerId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"game/{playerId}/perform-action";

            var httpResponse = await httpClient.PostAsync(route, null);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<GameResult>>();

            if (result is null)
            {
                return new ServiceResult<GameResult>();
            }

            return result;
        }

        public async Task<ServiceResult<BuyResult>> Buy(int playerId, int itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"game/{playerId}/buy/{itemId}";

            var httpResponse = await httpClient.PostAsync(route, null);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<BuyResult>>();

            if (result is null)
            {
                return new ServiceResult<BuyResult>();
            }

            return result;
        }
    }
}
