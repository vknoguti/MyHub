using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyHub.DTOs
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = nameof(UserName) + " is required")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = nameof(Password) + " is required")]
        public string Password { get; set; } = null!;

        [EmailAddress]
        [Required(ErrorMessage = nameof(Email) + " is required")]
        public string Email { get; set; } = null!;
    }
}
