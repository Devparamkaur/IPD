using AutoMapper;
using IPD.Application.DTOs;
using IPD.Application.DTOs.Identity;
using IPD.Application.Interfaces.Services;
using IPD.Infrastructure.Identity;
using IPD.Infrastructure.Identity.Interfaces;
using IPD.Web.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;

namespace IPD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public AccountsController(IMapper mapper,
               IAccountService accountService,
             ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _mapper = mapper;
            _accountService = accountService;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }


        #region Login

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="userForAuthentication"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForAuthenticationDto userForAuthentication)
        {

            var user = await _identityService.FindByNameAsync(userForAuthentication.Email);

            if (user == null || !await _identityService.CheckPasswordAsync(user, userForAuthentication.Password))
                return Unauthorized("Invalid Credentials");

            if (!user.EmailConfirmed)
                return Unauthorized("Email is not confirmed");

            return Ok();
        }


        /// <summary>
        /// LogOut user
        /// </summary>
        /// <returns></returns>
        [HttpPost("LogOut")]
        public async Task LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(JwtBearerDefaults.AuthenticationScheme);
        }

        #endregion

        #region Registeration


        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="userForRegistration"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var user = _mapper.Map<ApplicationUser>(userForRegistration);
            var result = await _identityService.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(e => e.Description).ToList());
            }

            return Ok();

        }


        #endregion

        /// <summary>
        /// Send confirmation email to the user.
        /// </summary>
        /// <param name="email">email address</param>
        /// <param name="token">token</param>
        /// <returns></returns>
        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _identityService.FindByEmailAsync(email);
            if (user == null)
                return BadRequest("Invalid Email Confirmation Request");
            var confirmResult = await _identityService.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest("Invalid Email Confirmation Request");
            return Ok();
        }


        /// <summary>
        /// Send forgot password email to the user
        /// </summary>
        /// <param name="forgotPasswordDto">forgotPasswordDto</param>
        /// <returns></returns>
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var user = await _identityService.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");
            var token = await _identityService.GeneratePasswordResetTokenAsync(user);

            return Ok(await _accountService.SendResetPasswordEmailAsync(user.Email, token, forgotPasswordDto.ClientURI));
        }


        /// <summary>
        /// Reset password
        /// </summary>
        /// <param name="resetPasswordDto"></param>
        /// <returns></returns>
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var user = await _identityService.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest("Invalid Request");

            var resetPassResult = await _identityService.ResetPasswordAsync(user, resetPasswordDto);
            if (!resetPassResult.Succeeded)
            {
                return BadRequest(resetPassResult.Errors.Select(e => e.Description));
            }

            return Ok(resetPassResult);
        }


        /// <summary>
        /// Resend confirmation email to the user
        /// </summary>
        /// <param name="email">email address</param>
        /// <param name="uri">uri of client page</param>
        /// <returns></returns>
        [HttpGet("ResendConfirmationEmail")]
        public async Task<IActionResult> ResendConfirmationEmail(string email, string uri)
        {
            var user = await _identityService.FindByEmailAsync(email);
            var token = await _identityService.GenerateEmailConfirmationTokenAsync(user);
            return Ok(await _accountService.SendConfirmationEmailAsync(user.FirstName, user.Email, token, uri));
        }
    }
}
