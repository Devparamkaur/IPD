using System.ComponentModel.DataAnnotations;

namespace IPD.Domain.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid Leader { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}
