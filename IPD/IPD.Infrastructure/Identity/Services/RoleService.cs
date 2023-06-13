using AutoMapper;
using IPD.Application.DTOs.Identity;
using IPD.Application.Interfaces.Services.Identity;
using IPD.Application.Responses.Identity;
using IPD.Infrastructure.Helpers;
using IPD.Infrastructure.Identity;
using IPD.Shared.Constants.Permission;
using IPD.Shared.Constants.Role;
using IPD.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IPD.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IMapper _mapper;

        public RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        /// Deletes role by id
        /// </summary>
        /// <param name="id">role id</param>
        /// <returns></returns>
        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != RoleConstant.AdministratorRole)
            {
                //TODO Check if Any Users already uses this Role
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    return Result<string>.Success($"Role {existingRole.Name} deleted.");
                }
                else
                {
                    return Result<string>.Fail($"Not allowed to delete {existingRole.Name} Role as it is being used.");
                }
            }
            else
            {
                return Result<string>.Fail($"Not allowed to delete {existingRole.Name} Role.");
            }
        }

        /// <summary>
        /// Gets all roles
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            return Result<List<RoleResponse>>.Success(rolesResponse);
        }
        

        /// <summary>
        /// Gets permissions by role id
        /// </summary>
        /// <param name="roleId">role id</param>
        /// <returns></returns>
        public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
        {
            var model = new PermissionResponse();
            var allPermissions = new List<RoleClaimsResponse>();

            #region GetPermissions

            allPermissions.GetPermissions(typeof(Permissions.Users), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Roles), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Brands), roleId);

            #endregion GetPermissions

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                model.RoleId = role.Id;
                model.RoleName = role.Name;
                var claims = await _roleManager.GetClaimsAsync(role);
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = claims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                    }
                }
            }
            model.RoleClaims = allPermissions;
            return Result<PermissionResponse>.Success(model);
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(Guid id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            var rolesResponse = _mapper.Map<RoleResponse>(roles);
            return Result<RoleResponse>.Success(rolesResponse);
        }

        public async Task<RoleResponse> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.Roles.FirstOrDefaultAsync(x => x.Id == id);
            var roleResponse = _mapper.Map<RoleResponse>(role);
            return roleResponse;
        }

        /// <summary>
        /// Updates  role
        /// </summary>
        /// <param name="request">role request object</param>
        /// <returns></returns>
        public async Task<Result<string>> SaveAsync(RoleDto request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null) return Result<string>.Fail($"Similar Role already exists.");
                var response = await _roleManager.CreateAsync(new ApplicationRole(request.Name));
                return Result<string>.Success("Role Created.");
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                //if (existingRole.Name == "Administrator" || existingRole.Name == "Basic")
                //{
                //    return Result<string>.Fail($"Not allowed to modify {existingRole.Name} Role.");
                //}
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                await _roleManager.UpdateAsync(existingRole);
                return Result<string>.Success("Role Updated.");
            }
        }


    
        /// <summary>
        /// Updates permissions
        /// </summary>
        /// <param name="request">permissions dto</param>
        /// <returns></returns>
        public async Task<Result<string>> UpdatePermissionsAsync(PermissionDto request)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role.Name == "Administrator")
                {
                    return Result<string>.Fail($"Not allowed to modify Permissions for this Role.");
                }
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                var selectedClaims = request.RoleClaims.Where(a => a.Selected).ToList();
                foreach (var claim in selectedClaims)
                {
                    await _roleManager.AddPermissionClaim(role, claim.Value);
                }
                return Result<string>.Success("Permission Updated.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }


        /// <summary>
        /// Gets count of roles
        /// </summary>
        /// <returns>total count</returns>
        public async Task<int> GetCountAsync()
        {
            var count = await _roleManager.Roles.CountAsync();
            return count;
        }
    }
}
