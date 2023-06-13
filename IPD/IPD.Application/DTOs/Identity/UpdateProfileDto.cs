using System.ComponentModel.DataAnnotations;

namespace IPD.Application.DTOs.Identity
{
    public class UpdateProfileDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
