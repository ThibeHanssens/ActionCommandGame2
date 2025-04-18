using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Api.Authentication.Settings;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ActionCommandGame.Api.Authentication
{
	public class IdentityService: IIdentityService<AuthenticationResult>
    {
		private readonly UserManager<IdentityUser> _userManager;
		private readonly JwtSettings _jwtSettings;

		public IdentityService(
			UserManager<IdentityUser> userManager, 
			JwtSettings jwtSettings)
		{
			_userManager = userManager;
			_jwtSettings = jwtSettings;
		}

        public async Task<AuthenticationResult> Register(UserRegistrationRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user is not null)
			{
				return new AuthenticationResult
				{
					Errors = new List<string> { "Registration failed" }
				};
			}

			user = new IdentityUser
			{
				Email = request.Email,
				UserName = request.Email
			};
			var result = await _userManager.CreateAsync(user, request.Password);
			if (!result.Succeeded)
			{
				return new AuthenticationResult
				{
					Errors = result.Errors.Select(e => e.Description)
				};
			}

			return GenerateAuthenticationResult(user);
		}

		public async Task<AuthenticationResult> SignIn(UserSignInRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);
			if (user is null)
			{
				return new AuthenticationResult
				{
					Errors = new List<string> { "User/password combination is wrong" }
				};
			}

			var hasValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
			
			if (!hasValidPassword)
			{
				return new AuthenticationResult
				{
					Errors = new List<string> { "User/password combination is wrong" }
				};
			}

			return GenerateAuthenticationResult(user);
		}

		private AuthenticationResult GenerateAuthenticationResult(IdentityUser user)
		{
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
            {
                return new AuthenticationResult { Errors = new List<string> { "Internal configuration error" } };
            }

			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id)
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Email));
				claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return new AuthenticationResult
			{
				Token = tokenHandler.WriteToken(token)
			};
		}
	}
}
