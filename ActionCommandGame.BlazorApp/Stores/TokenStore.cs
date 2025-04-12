using Blazored.LocalStorage;
using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.BlazorApp.Stores
{
    public class TokenStore : ITokenStore
    {
        private const string TokenKey = "JwtToken";
        private readonly ILocalStorageService _localStorage;

        public TokenStore(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<string?> GetTokenAsync()
        {
            // Retrieves the token from local storage, returning an empty string if not found.
            return await _localStorage.GetItemAsync<string>(TokenKey) ?? string.Empty;
        }

        public async Task SaveTokenAsync(string? token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                // Remove token if it is empty.
                await _localStorage.RemoveItemAsync(TokenKey);
            }
            else
            {
                // Save token to local storage.
                await _localStorage.SetItemAsync(TokenKey, token);
            }
        }
    }
}
