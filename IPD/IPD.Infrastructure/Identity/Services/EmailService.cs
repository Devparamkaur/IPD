using Azure.Storage.Blobs;
using IPD.Application.Enums;
using IPD.Application.Interfaces.Services;
using IPD.Infrastructure.Azure;
using IPD.Infrastructure.Emails.Helpers;
using IPD.Infrastructure.Emails.SendGrid;
using IPD.Shared.Wrapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;

namespace IPD.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<EmailSettings> _options;
        public EmailService(IConfiguration configuration, IOptions<EmailSettings> options)
        {
            _configuration = configuration;
            _options = options;
        }


        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="recepiants">recepiants of email</param>
        /// <param name="subject">subject</param>
        /// <param name="htmlMessage">message</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string[] recepiants, string subject, string htmlMessage)
        {
            string? fromEmail = _options.Value.SenderEmail;
            string? fromName = _options.Value.SenderName;
            string? apiKey = _options.Value.ApiKey;
            var sendGridClient = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(string.Join(",", recepiants));
            var plainTextContent = Regex.Replace(htmlMessage, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject,
            plainTextContent, htmlMessage);
            var response = await sendGridClient.SendEmailAsync(msg);
        }

        /// <summary>
        /// Send email template
        /// </summary>
        /// <param name="replacements">replacements</param>
        /// <param name="recepiants">recepiants of mail</param>
        /// <returns></returns>
        public async Task SendTemplateEmailAsync(object replacements, params string[] recepiants)
        {
            string? fromEmail = _options.Value.SenderEmail;
            string? fromName = _options.Value.SenderName;
            string? apiKey = _options.Value.ApiKey;
            var sendGridClient = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(string.Join(",", recepiants));

            var msg = MailHelper.CreateSingleTemplateEmail(from, to, SendGridDynamicTemplates.White, replacements);

            var response = await sendGridClient.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Email has been sent successfully");
            }
        }






        /// <summary>
        /// Gets email body from saved templates.
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="subType"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<string> GetEmailBody(EmailTemplateTypes emailType, string subType = null, string role = null)
        {
            BlobContainerClient container = AzureBlobFileService.InitBlobContainer(ContainerNames.EmailTemplates);

            BlobClient templateBlob = null;
            BlobClient tempBlob = null;

            if (!string.IsNullOrEmpty(subType))
            {
                if (!string.IsNullOrEmpty(role))
                {
                    templateBlob =
                        container.GetBlobClient(string.Format("{0}.{1}[{2}].template",
                            emailType.ToString().ToLower(), subType.ToLower(), role.ToLower()));

                    tempBlob =
                        container.GetBlobClient(string.Format("{0}.{1}[{2}].subject",
                            emailType.ToString().ToLower(), subType.ToLower(), role.ToLower()));

                    if (!await templateBlob.ExistsAsync())
                    {
                        templateBlob = null;
                    }
                }
                else
                {
                    templateBlob =
                        container.GetBlobClient(string.Format("{0}.{1}.template", emailType.ToString().ToLower(),
                            subType.ToLower()));

                    tempBlob =
                       container.GetBlobClient(string.Format("{0}.{1}.subject", emailType.ToString().ToLower(),
                           subType.ToLower()));

                    if (!await templateBlob.ExistsAsync())
                    {
                        templateBlob = null;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(role))
            {
                templateBlob =
                    container.GetBlobClient(string.Format("{0}[{1}].template", emailType.ToString().ToLower(),
                        role.ToLower()));

                tempBlob =
                    container.GetBlobClient(string.Format("{0}[{1}].subject", emailType.ToString().ToLower(),
                        role.ToLower()));

                if (!await templateBlob.ExistsAsync())
                {
                    templateBlob = null;
                }
            }

            if (templateBlob == null)
            {
                templateBlob =
                    container.GetBlobClient(string.Format("{0}.template", emailType.ToString().ToLower()));
                tempBlob =
                    container.GetBlobClient(string.Format("{0}.subject", emailType.ToString().ToLower()));
            }

            if (await templateBlob.ExistsAsync())
            {
                var bodyContent = string.Empty;

                using (var stream = new MemoryStream())
                {
                    await templateBlob.DownloadToAsync(stream);
                    using (var reader = new StreamReader(stream, true))
                    {
                        stream.Position = 0;
                        bodyContent = reader.ReadToEnd();
                    }
                }

                return bodyContent;

            }
            return string.Empty;
        }



        /// <summary>
        /// Send email template
        /// </summary>
        /// <param name="emailType"></param>
        /// <param name="bodyReplacementValues"></param>
        /// <param name="customizedSubTemplate"></param>
        /// <param name="customizedRoleTemplate"></param>
        /// <param name="customSubject"></param>
        /// <param name="customSender"></param>
        /// <param name="recepiants"></param>
        /// <returns></returns>
        public async Task<IResult> SendEmailTemplateAsync(EmailTemplateTypes emailType, IDictionary<string, string> bodyReplacementValues, string customizedSubTemplate = null, string customizedRoleTemplate = null, string customSubject = null, string customSender = null, params string[] recepiants)
        {
            var template = await GetEmailBody(emailType, customizedSubTemplate, customizedRoleTemplate);
            var emailBody = EmailHelper.GetBodyAfterReplacements(bodyReplacementValues, template);

            var dynamicTemplateData = new
            {
                subject = customSubject,
                BODY = emailBody
            };

            await SendTemplateEmailAsync(dynamicTemplateData, recepiants);
            return Result.Success();
        }

    }
}