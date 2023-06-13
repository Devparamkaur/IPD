using IPD.Application.Responses.Identity;
using IPD.Infrastructure.Identity;
using IPD.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using System.Security.Claims;

namespace IPD.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetPermissions(this List<RoleClaimsResponse> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsResponse { Value = fi.GetValue(null).ToString(), Type = ApplicationClaimTypes.Permission });
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }
        }

        public static async Task GeneratePermissionClaimByModule(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = PermissionModules.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
                }
            }
        }
    }
}
