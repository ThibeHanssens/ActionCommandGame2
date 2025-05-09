﻿using System.Net.Http.Json;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class ItemSdk: IItemService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public ItemSdk(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<IList<ItemResult>>> Find()
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = "items";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<IList<ItemResult>>>();

            if (result is null)
            {
                return new ServiceResult<IList<ItemResult>>(new List<ItemResult>());
            }

            return result;
        }

        public async Task<ServiceResult<ItemResult>> Get(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"items/{id}";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<ItemResult>>();

            if (result is null)
            {
                return new ServiceResult<ItemResult>();
            }

            return result;
        }

        // Create a new item
        public async Task<ServiceResult<ItemResult>> Create(ItemCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.PostAsJsonAsync("admin/items", request);
            resp.EnsureSuccessStatusCode();
            return await resp.Content
                             .ReadFromJsonAsync<ServiceResult<ItemResult>>()
                   ?? new ServiceResult<ItemResult>();
        }

        // Update an existing item
        public async Task<ServiceResult<ItemResult>> Update(ItemUpdateRequest request)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.PutAsJsonAsync($"admin/items/{request.Id}", request);
            resp.EnsureSuccessStatusCode();
            return await resp.Content
                             .ReadFromJsonAsync<ServiceResult<ItemResult>>()
                   ?? new ServiceResult<ItemResult>();
        }

        // Delete an item
        public async Task<ServiceResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("ActionCommandGame");
            client.AddAuthorization(await _tokenStore.GetTokenAsync());
            var resp = await client.DeleteAsync($"admin/items/{id}");
            resp.EnsureSuccessStatusCode();
            return await resp.Content
                             .ReadFromJsonAsync<ServiceResult>()
                   ?? new ServiceResult();
        }
    }
}
