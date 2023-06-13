using Microsoft.Extensions.Configuration;

namespace IPD.Infrastructure.Helpers
{
    public static class AppSettingsHelper
    {
        private static IConfiguration _configuration;
        public static void AppSettingsConfigure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string Setting(string key)
        {
            return _configuration.GetSection(key).Value;
        }
    }
}
