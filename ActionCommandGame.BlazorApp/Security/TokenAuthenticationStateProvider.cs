using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using ActionCommandGame.Sdk.Abstractions;
using System.IdentityModel.Tokens.Jwt;

namespace ActionCommandGame.BlazorApp.Security
{
    public class TokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenStore _tokenStore;

        public TokenAuthenticationStateProvider(ITokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return await GetAuthenticationStateFromTokenAsync();
        }
        public async void AuthenticateUser()
        {
            var authenticationState = await GetAuthenticationStateFromTokenAsync();
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }

        public async Task<AuthenticationState> GetAuthenticationStateFromTokenAsync()
        {
            var token = await _tokenStore.GetTokenAsync();

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var jwtToken = new JsonWebToken(token);
                var claims = jwtToken.Claims.Select(c => new Claim(c.Type, c.Value)).ToList();
                //var identity = new ClaimsIdentity(claims, "jwt");
                var identity = new ClaimsIdentity(claims, authenticationType: "jwt", nameType: System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, roleType: "role");
                var user = new ClaimsPrincipal(identity);
                return new AuthenticationState(user);
            }
            catch
            {
                // Handle corrupted tokens.
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }
    }
}
