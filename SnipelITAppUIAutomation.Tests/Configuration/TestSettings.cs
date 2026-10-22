using Microsoft.Extensions.Configuration;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Configuration
{
    public sealed class TestSettings
    {
        private const int DefaultTimeoutSeconds = 30;

        private TestSettings(Uri baseUrl, string username, string password, TimeSpan timeout)
        {
            BaseUrl = baseUrl;
            Username = username;
            Password = password;
            Timeout = timeout;
        }

        public Uri BaseUrl { get; }

        public string Username { get; }

        public string Password { get; }

        public TimeSpan Timeout { get; }

        public static TestSettings Load()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<TestSettings>(optional: true)
                .AddEnvironmentVariables()
                .Build();

            var baseUrl = ParseBaseUrl(
                GetRequiredSetting(configuration, "SNIPEIT_BASE_URL"));
            var username = GetRequiredSetting(configuration, "SNIPEIT_USERNAME");
            var password = GetRequiredSetting(configuration, "SNIPEIT_PASSWORD");
            var timeoutValue = configuration["SNIPEIT_TIMEOUT_SECONDS"];
            var timeoutSeconds = DefaultTimeoutSeconds;
            if (!string.IsNullOrWhiteSpace(timeoutValue) &&
                (!int.TryParse(timeoutValue, out timeoutSeconds) || timeoutSeconds <= 0))
            {
                throw new InvalidOperationException(
                    "SNIPEIT_TIMEOUT_SECONDS must be a positive whole number.");
            }

            return new TestSettings(baseUrl, username, password, TimeSpan.FromSeconds(timeoutSeconds));
        }

        private static Uri ParseBaseUrl(string value)
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out var baseUrl) ||
                (baseUrl.Scheme != Uri.UriSchemeHttps && baseUrl.Scheme != Uri.UriSchemeHttp))
            {
                throw new InvalidOperationException(
                    "SNIPEIT_BASE_URL must be an absolute HTTP or HTTPS URL.");
            }

            return baseUrl;
        }

        private static string GetRequiredSetting(IConfiguration configuration, string name)
        {
            var value = configuration[name];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Required setting {name} is missing or empty.");
            }

            return value;
        }
    }

}
