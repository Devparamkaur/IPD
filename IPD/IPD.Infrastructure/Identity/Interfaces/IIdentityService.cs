using IPD.Application.DTOs.Identity;
using IPD.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;

namespace IPD.Infrastructure.Identity.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> AuthorizeAsync(Guid userId, string policyName);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
        Task<(IResult Result, Guid UserId)> CreateUserAsync(string userName, string password);
        Task<IResult> DeleteUserAsync(ApplicationUser user);
        Task<IResult> DeleteUserAsync(Guid userId);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser?> FindByNameAsync(string email);
        Task<ApplicationUser> FindByIdAsync(Guid id);
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
        Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
        Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, ResetPasswordDto resetPasswordDto);
        Task<IdentityResult> SetEmailAsync(ApplicationUser user, string email);
        Task<IdentityResult> SetUserNameAsync(ApplicationUser user, string userName);
        Task<string?> GetUserNameAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role);
        Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role);
    }
}