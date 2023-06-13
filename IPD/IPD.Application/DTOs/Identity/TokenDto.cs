using System.ComponentModel.DataAnnotations;

namespace IPD.Application.DTOs.Identity
{
    public class TokenDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
