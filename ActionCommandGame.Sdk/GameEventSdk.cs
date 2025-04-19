using System.Net.Http.Json;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Services.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Sdk
{
    public class GameEventSdk
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public GameEventSdk(IHttpClientFactory factory, ITokenStore tokens)
        {
            _httpClientFactory = factory;
            _tokenStore = tokens;
        }

        // Positive events
        public async Task<ServiceResult<IList<PositiveGameEventResult>>> FindPositive()
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.GetAsync("game-events/positive");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ServiceResult<IList<PositiveGameEventResult>>>()
                   ?? new ServiceResult<IList<PositiveGameEventResult>>();
        }

        public async Task<ServiceResult<PositiveGameEventResult>> CreatePositive(PositiveGameEventResult dto)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.PostAsJsonAsync("game-events/positive", dto);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ServiceResult<PositiveGameEventResult>>()
                   ?? new ServiceResult<PositiveGameEventResult>();
        }

        public async Task<ServiceResult> DeletePositive(int id)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.DeleteAsync($"game-events/positive/{id}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ServiceResult>()
                   ?? new ServiceResult();
        }

        // Negative events
        public async Task<ServiceResult<IList<NegativeGameEventResult>>> FindNegative()
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.GetAsync("game-events/negative");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ServiceResult<IList<NegativeGameEventResult>>>()
                   ?? new ServiceResult<IList<NegativeGameEventResult>>();
        }

        public async Task<ServiceResult<NegativeGameEventResult>> CreateNegative(NegativeGameEventResult dto)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.PostAsJsonAsync("game-events/negative", dto);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ServiceResult<NegativeGameEventResult>>()
                   ?? new ServiceResult<NegativeGameEventResult>();
        }

        public async Task<ServiceResult> DeleteNegative(int id)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.DeleteAsync($"game-events/negative/{id}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadFromJsonAsync<ServiceResult>()
                   ?? new ServiceResult();
        }
    }
}
