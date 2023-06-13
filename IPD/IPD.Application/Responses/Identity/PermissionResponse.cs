namespace IPD.Application.Responses.Identity
{
    public class PermissionResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleClaimsResponse> RoleClaims { get; set; }
    }
}