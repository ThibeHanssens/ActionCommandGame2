using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Services.Model.Requests
{
    public class UserProfileUpdateRequest
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        // only used if the user wants to change password
        public string? CurrentPassword { get; set; }

        public string? NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }
    }
}
