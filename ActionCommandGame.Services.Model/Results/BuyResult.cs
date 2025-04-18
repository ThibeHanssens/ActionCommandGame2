namespace ActionCommandGame.Services.Model.Results
{
    public class UserProfileResult
    {
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? PhoneNumber { get; set; }
    }
}
