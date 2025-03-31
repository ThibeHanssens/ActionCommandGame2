using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.Ui.MvcApplication.Stores
{
    public class TokenStore : ITokenStore
    {
        private const string TokenName = "JwtToken";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenStore(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string?> GetTokenAsync()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return Task.FromResult<string?>(string.Empty);
            }

            _httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(TokenName, out string? token);

            var tokenResult = token ?? string.Empty;
            return Task.FromResult<string?>(tokenResult);
        }

        public Task SaveTokenAsync(string? token)
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return Task.FromResult(string.Empty);
            }

            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey(TokenName))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Delete(TokenName);
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpContextAccessor.HttpContext.Response.Cookies.Append(TokenName, token,
                    new CookieOptions { HttpOnly = true });
            }

            return Task.CompletedTask;
        }
    }
}
