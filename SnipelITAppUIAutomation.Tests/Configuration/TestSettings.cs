using Microsoft.Extensions.Configuration;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Configuration
{
    public sealed class TestSettings
    {
        private const string DefaultBaseUrl = "https://demo.snipeitapp.com";
        private const int DefaultTimeoutSeconds = 30;

        private TestSettings(Uri baseUrl, string username, string password, TimeSpan timeout, bool headless)
        {
            BaseUrl = baseUrl;
            Username = username;
            Password = password;
            Timeout = timeout;
            Headless = headless;
        }

        public Uri BaseUrl { get; }

        public string Username { get; }

        public string Password { get; }

        public TimeSpan Timeout { get; }

        public bool Headless { get; }

        public static TestSettings Load()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: true)
                .AddEnvironmentVariables()
                .Build();

            var baseUrl = ParseBaseUrl(configuration["SNIPEIT_BASE_URL"] ?? DefaultBaseUrl);
            var username = configuration["SNIPEIT_USERNAME"] ?? GetRequiredEnvironmentVariable("SNIPEIT_USERNAME");
            var password = configuration["SNIPEIT_PASSWORD"] ?? GetRequiredEnvironmentVariable("SNIPEIT_PASSWORD");
            var timeoutValue = configuration["SNIPEIT_TIMEOUT_SECONDS"];
            var timeoutSeconds = DefaultTimeoutSeconds;
            if (!string.IsNullOrWhiteSpace(timeoutValue) &&
                (!int.TryParse(timeoutValue, out timeoutSeconds) || timeoutSeconds <= 0))
            {
                throw new InvalidOperationException(
                    "SNIPEIT_TIMEOUT_SECONDS must be a positive whole number.");
            }

            var headlessValue = configuration["SNIPEIT_HEADLESS"];
            var headless = false;
            if (!string.IsNullOrWhiteSpace(headlessValue) &&
                !bool.TryParse(headlessValue, out headless))
            {
                throw new InvalidOperationException("SNIPEIT_HEADLESS must be 'true' or 'false'.");
            }

            return new TestSettings(baseUrl, username, password, TimeSpan.FromSeconds(timeoutSeconds), headless);
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

        private static string GetRequiredEnvironmentVariable(string name)
        {
            var value = Environment.GetEnvironmentVariable(name);
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Required environment variable {name} is missing or empty.");
            }

            return value;
        }
    }

}
