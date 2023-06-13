using IPD.Application.Attributes;

namespace IPD.Application.Enums
{
    public enum EmailTemplateTypes
    {
        None,
        [EmailDefaults("", "Thank you for registering with IPD")]
        Registration,

        ForgotPassword
    }
}
