using IPD.Application.Attributes;
using IPD.Application.Enums;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IPD.Infrastructure.Emails.Helpers
{
    public static class EmailHelper
    {

        /// <summary>
        /// Gets email body after replace provided placeholders.
        /// </summary>
        /// <param name="replacements">replacements</param>
        /// <param name="body">email body</param>
        /// <returns>email body</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetBodyAfterReplacements(IDictionary<string, string> replacements, string body)
        {
            if (replacements != null && !string.IsNullOrEmpty(body))
            {
                foreach (object key in replacements.Keys)
                {
                    string text = key as string;
                    string text2 = replacements[key.ToString()] as string;
                    if (text == null || text2 == null)
                    {
                        throw new ArgumentException();
                    }

                    text2 = text2.Replace("$", "$$");
                    body = Regex.Replace(body, text, text2, RegexOptions.IgnoreCase);
                }
            }
            return body;
        }

        /// <summary>
        /// Gets defaulr email attributes
        /// </summary>
        /// <param name="value">type of email template</param>
        /// <returns>attributes</returns>
        public static EmailDefaultsAttribute GetDefaultEmailAttribute(this EmailTemplateTypes value)
        {
            Type type = value.GetType();

            FieldInfo fieldInfo = type.GetField(value.ToString());

            EmailDefaultsAttribute[] attributes = fieldInfo.GetCustomAttributes(typeof(EmailDefaultsAttribute), false) as EmailDefaultsAttribute[];

            return attributes.Length > 0 ? attributes[0] : null;
        }
    }
}
