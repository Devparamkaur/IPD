using IPD.Application.Enums;
using IPD.Shared.Wrapper;

namespace IPD.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string[] recepiants, string subject, string htmlMessage);
        Task SendTemplateEmailAsync(object replacements, params string[] recepiants);
        Task<string> GetEmailBody(EmailTemplateTypes emailType, string subType = null, string role = null);
        Task<IResult> SendEmailTemplateAsync(EmailTemplateTypes emailType, IDictionary<string, string> bodyReplacementValues, string customizedSubTemplate = null, string customizedRoleTemplate = null, string customSubject = null, string customSender = null, params string[] recepiants);
    }
}
