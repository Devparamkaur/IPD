using System.ComponentModel.DataAnnotations;

namespace IPD.Application.DTOs.Identity
{
    public class RoleDto
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
