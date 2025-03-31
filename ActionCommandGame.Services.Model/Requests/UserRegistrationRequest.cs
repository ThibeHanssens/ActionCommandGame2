namespace ActionCommandGame.Services.Model.Requests
{
	public class UserRegistrationRequest
	{
		public required string Email { get; set; }

		public required string Password { get; set; }
	}
}
