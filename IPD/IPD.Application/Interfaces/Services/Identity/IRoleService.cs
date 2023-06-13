using IPD.Application.DTOs.Identity;
using IPD.Application.Interfaces.Common;
using IPD.Application.Responses.Identity;
using IPD.Shared.Wrapper;

namespace IPD.Application.Interfaces.Services.Identity
{
    public interface IRoleService : IService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(Guid id);

        Task<Result<string>> SaveAsync(RoleDto request);

        Task<Result<string>> DeleteAsync(string id);

        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

        Task<Result<string>> UpdatePermissionsAsync(PermissionDto request);
    }
}
