using System.ComponentModel.DataAnnotations;

namespace IPD.Application.DTOs.Identity
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
