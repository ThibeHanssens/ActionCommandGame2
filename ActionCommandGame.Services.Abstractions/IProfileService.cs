using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IProfileService
    {
        Task<UserProfileResult?> GetProfileAsync();
        Task<AuthenticationResult> UpdateProfileAsync(UserProfileUpdateRequest request);
    }
}
