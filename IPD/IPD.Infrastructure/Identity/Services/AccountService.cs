using IPD.Application.DTOs.Identity;
using IPD.Application.Enums;
using IPD.Application.Interfaces.Services;
using IPD.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace IPD.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IEmailService _emailService;

        #region ctor
        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #endregion

        /// <summary>
        /// Changes password for user
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<IResult> ChangePasswordAsync(ChangePasswordDto model, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail("User Not Found.");
            }

            var identityResult = await this._userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return identityResult.Succeeded ? Result.Success() : Result.Fail(errors);
        }


        /// <summary>
        /// Updates the user's basic info
        /// </summary>
        /// <param name="model">model</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<IResult> UpdateProfileBasicInfoAsync(UpdateProfileDto model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail("User Not Found.");
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            }
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            await _signInManager.RefreshSignInAsync(user);
            return identityResult.Succeeded ? Result.Success() : Result.Fail(errors);
        }


        /// <summary>
        /// Gets profile picture of user
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Result<string>.Fail("User Not Found");

            return Result<string>.Success("");
        }


        /// <summary>
        /// Updates profile picture of user
        /// </summary>
        /// <param name="request">request dto</param>
        /// <param name="userId">user id</param>
        /// <returns></returns>
        public async Task<IResult> UpdateProfilePictureAsync(ProfilePictureDto request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return Result.Fail("User Not Found");

            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return identityResult.Succeeded ? Result.Success() : Result.Fail(errors);
        }


        /// <summary>
        /// Send confirmation email to  user
        /// </summary>
        /// <param name="firstName">first name of user</param>
        /// <param name="email">email address</param>
        /// <param name="emailConfirmationToken">token</param>
        /// <param name="clientURI">client uri to receove response</param>
        /// <returns></returns>
        public async Task<IResult> SendConfirmationEmailAsync(string firstName, string email, string emailConfirmationToken, string clientURI)
        {
            var param = new Dictionary<string, string?>
                    {
                        {"token", emailConfirmationToken },
                        {"email", email }
                    };
            var callback = QueryHelpers.AddQueryString(clientURI, param);

            var replacements = new Dictionary<string, string>
                                                        {
                                                            { "#Firstname#", firstName},
                                                            { "#ConfirmationLink#" ,callback},
                                                        };

            await _emailService.SendEmailTemplateAsync(EmailTemplateTypes.Registration, replacements, "emailverification", "user", "Register Email Verification", null, email);

            return Result.Success();
        }


        /// <summary>
        /// Send reset password email to user
        /// </summary>
        /// <param name="email">email address</param>
        /// <param name="token">token</param>
        /// <param name="clientURI">client uri to receove response</param>
        /// <returns></returns>
        public async Task<IResult> SendResetPasswordEmailAsync(string email, string token, string clientURI)
        {

            var param = new Dictionary<string, string?>
              {
                {"token", token },
                {"email", email }
              };
            var callback = QueryHelpers.AddQueryString(clientURI, param);

            var replacements = new Dictionary<string, string>
                                                        {
                                                            { "#Link#" ,callback }
                                                        };

            await _emailService.SendEmailTemplateAsync(EmailTemplateTypes.ForgotPassword, replacements, null, null, "Reset Password", null, email);

            return Result<bool>.Success();
        }
    }
}
