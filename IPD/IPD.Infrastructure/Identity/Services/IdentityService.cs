using IPD.Application.DTOs.Identity;
using IPD.Infrastructure.Identity.Interfaces;
using IPD.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IPD.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        // private readonly IAuthorizationService _authorizationService;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory
            /*IAuthorizationService authorizationService*/)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            // _authorizationService = authorizationService;
        }


        /// <summary>
        /// Gets username
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>username</returns>
        public async Task<string?> GetUserNameAsync(Guid userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }


        /// <summary>
        /// Finds user by name
        /// </summary>
        /// <param name="name">username</param>
        /// <returns>user</returns>
        public async Task<ApplicationUser?> FindByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }


        /// <summary>
        /// Finds user by email
        /// </summary>
        /// <param name="email">email address</param>
        /// <returns>user</returns>
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }


        /// <summary>
        /// Finds user by id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user</returns>
        public async Task<ApplicationUser> FindByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }


        /// <summary>
        /// Authenticates user by password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="password">password</param>
        /// <returns>authentication result</returns>
        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }


        /// <summary>
        /// Confirms email
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="token">token</param>
        /// <returns>result</returns>
        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }


        /// <summary>
        /// Resets password
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="resetPasswordDto"> dto</param>
        /// <returns>result</returns>
        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, ResetPasswordDto resetPasswordDto)
        {
            return await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
        }

        /// <summary>
        /// Creates user 
        /// </summary>
        /// <param name="user">user object</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }


        /// <summary>
        /// Generates email confirmation token
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>generated token</returns>
        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }


        /// <summary>
        /// Updates email for user
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="email">email address</param>
        /// <returns>result</returns>
        public async Task<IdentityResult> SetEmailAsync(ApplicationUser user, string email)
        {
            return await _userManager.SetEmailAsync(user, email);
        }

        /// <summary>
        /// Updates username
        /// </summary>
        /// <param name="user">user</param>
        /// <param name="userName">username</param>
        /// <returns>result</returns>
        public async Task<IdentityResult> SetUserNameAsync(ApplicationUser user, string userName)
        {
            return await _userManager.SetUserNameAsync(user, userName);
        }


        /// <summary>
        /// Generates password reset token
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>token</returns>
        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }


        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="userName">username</param>
        /// <param name="password">password</param>
        /// <returns></returns>
        public async Task<(IResult Result, Guid UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }


        /// <summary>
        /// Checks if user is in role
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="role">role</param>
        /// <returns></returns>
        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        /// <summary>
        /// Authorizes user by policy
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="policyName">policy name</param>
        /// <returns></returns>
        public async Task<bool> AuthorizeAsync(Guid userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            //var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            //return result.Succeeded;
            return true;
        }


        /// <summary>
        /// Deletes user by id
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<IResult> DeleteUserAsync(Guid userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }


        /// <summary>
        /// Deletes user by ApplicationUser object
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IResult> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }


        /// <summary>
        /// Gets roles for provided user
        /// </summary>
        /// <param name="user">ApplicationUser object</param>
        /// <returns></returns>
        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }


        /// <summary>
        /// Removes user from role
        /// </summary>
        /// <param name="user">ApplicationUser object</param>
        /// <param name="role">role</param>
        /// <returns></returns>
        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        /// <summary>
        /// Adds user to the role
        /// </summary>
        /// <param name="user">ApplicationUser object</param>
        /// <param name="role">role</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }
    }
}
