using System.ComponentModel.DataAnnotations;

namespace MyHub.DTOs.Authentication
{
    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

    }
}
