using System.ComponentModel.DataAnnotations;

namespace IPD.Domain.Entities
{
    public partial class Member
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string Phone { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}
