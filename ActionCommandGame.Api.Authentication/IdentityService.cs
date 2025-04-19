using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Api.Authentication.Settings;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Core;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ActionCommandGame.Api.Authentication
{
	public class IdentityService: IIdentityService<AuthenticationResult>
    {
        private readonly ActionCommandGameDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
		private readonly JwtSettings _jwtSettings;

		public IdentityService(
			UserManager<IdentityUser> userManager, 
			JwtSettings jwtSettings,
            ActionCommandGameDbContext db)
        {
			_userManager = userManager;
			_jwtSettings = jwtSettings;
			_db = db;
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

            // Create the default player for this new user**
            _db.Players.Add(new Player
            {
                UserId = user.Id,
                Name = "Default player",
                Money = 100,
                Experience = 0
            });
            await _db.SaveChangesAsync();

            return await GenerateAuthenticationResultAsync(user);
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

			return await GenerateAuthenticationResultAsync(user);
		}

		private async Task<AuthenticationResult> GenerateAuthenticationResultAsync(IdentityUser user)
		{
            if (string.IsNullOrWhiteSpace(_jwtSettings.Secret))
            {
                return new AuthenticationResult { Errors = new List<string> { "Internal configuration error" } };
            }

            // 1) base claims
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

            // 2) add all the user's roles as ClaimTypes.Role
            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // 3) create the token
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key),
                                               SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = creds
            };

            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateToken(tokenDescriptor);
            var token = handler.WriteToken(securityToken);

            return new AuthenticationResult { Token = token };
        }
	}
}
