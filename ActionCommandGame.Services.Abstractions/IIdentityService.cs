using ActionCommandGame.Services.Model.Requests;

namespace ActionCommandGame.Services.Abstractions
{
	public interface IIdentityService<TResult>
	{
		Task<TResult> Register(UserRegistrationRequest request);
		Task<TResult> SignIn(UserSignInRequest request);
	}
}
