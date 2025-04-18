using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
	public interface IIdentityService<TResult>
	{
        Task<TResult> Register(UserRegistrationRequest request);
		Task<TResult> SignIn(UserSignInRequest request);
    }
}
