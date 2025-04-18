using System.Threading.Tasks;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Api.Authentication.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ActionCommandGame.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProfileService(UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetUserId()
        => _httpContextAccessor
          .HttpContext?
          .User?
          .FindFirst("id")?
          .Value;

        public async Task<UserProfileResult?> GetProfileAsync()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var phone = await _userManager.GetPhoneNumberAsync(user);
            return new UserProfileResult
            {
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                PhoneNumber = phone
            };
        }

        public async Task<AuthenticationResult> UpdateProfileAsync(UserProfileUpdateRequest request)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User not found" }
                };
            }

            var errors = new List<string>();

            // 1) user name
            if (request.UserName != user.UserName)
            {
                var nameResult = await _userManager.SetUserNameAsync(user, request.UserName);
                if (!nameResult.Succeeded)
                    errors.AddRange(nameResult.Errors.Select(e => e.Description));
            }

            // 2) email
            if (request.Email != user.Email)
            {
                // check if someone else already has that email
                var existing = await _userManager.FindByEmailAsync(request.Email);
                if (existing != null && existing.Id != user.Id)
                {
                    errors.Add("That email address is already in use.");
                }
                else
                {
                    var emailResult = await _userManager.SetEmailAsync(user, request.Email);
                    if (!emailResult.Succeeded)
                        errors.AddRange(emailResult.Errors.Select(e => e.Description));
                }
            }

            // 3) phone
            var currentPhone = await _userManager.GetPhoneNumberAsync(user);
            if (!string.IsNullOrWhiteSpace(request.PhoneNumber)
             && request.PhoneNumber != currentPhone)
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, request.PhoneNumber);
                if (!phoneResult.Succeeded)
                    errors.AddRange(phoneResult.Errors.Select(e => e.Description));
            }

            // 4) password
            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                if (string.IsNullOrEmpty(request.CurrentPassword))
                {
                    errors.Add("Current password is required to set a new password.");
                }
                else
                {
                    var pwdResult = await _userManager.ChangePasswordAsync(
                        user, request.CurrentPassword, request.NewPassword);
                    if (!pwdResult.Succeeded)
                        errors.AddRange(pwdResult.Errors.Select(e => e.Description));
                }
            }

            if (errors.Any())
            {
                return new AuthenticationResult
                {
                    Errors = errors
                };
            }

            // no token on profile‑update, but the consumer can check .Success
            return new AuthenticationResult
            {
                Errors = Array.Empty<string>()
            };
        }
    }
}