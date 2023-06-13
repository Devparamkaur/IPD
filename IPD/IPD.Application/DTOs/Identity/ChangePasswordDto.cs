using System.ComponentModel.DataAnnotations;

namespace IPD.Application.DTOs.Identity
{
    public class ChangePasswordDto
    {
        [Required]
        public string Password { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
