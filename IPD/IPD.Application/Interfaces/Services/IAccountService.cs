using IPD.Application.DTOs.Identity;
using IPD.Shared.Wrapper;

namespace IPD.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<IResult> UpdateProfileBasicInfoAsync(UpdateProfileDto model, string userId);
        Task<IResult> ChangePasswordAsync(ChangePasswordDto model, string userId);
        Task<IResult<string>> GetProfilePictureAsync(string userId);
        Task<IResult> UpdateProfilePictureAsync(ProfilePictureDto request, string userId);
        Task<IResult> SendConfirmationEmailAsync(string firstName, string email, string emailConfirmationToken, string clientURI);
        Task<IResult> SendResetPasswordEmailAsync(string email, string token, string clientURI);
    }
}
