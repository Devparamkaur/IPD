namespace IPD.Application.DTOs.Identity
{
    public class PermissionDto
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsDto> RoleClaims { get; set; }
    }
}
