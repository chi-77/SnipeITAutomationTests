using Microsoft.Extensions.Configuration;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Configuration
{
    public sealed class TestData
    {
        private TestData(AssetCreationData assetCreation)
        {
            AssetCreation = assetCreation;
        }

        public AssetCreationData AssetCreation { get; }

        public static TestData Load()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "testdata.json"), optional: false)
                .Build();

            var modelName = GetRequired(configuration, "AssetCreation:ModelName");
            var statusName = GetRequired(configuration, "AssetCreation:StatusName");

            return new TestData(new AssetCreationData(modelName, statusName));
        }

        private static string GetRequired(IConfiguration configuration, string key)
        {
            var value = configuration[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Required test data setting '{key}' is missing or empty.");
            }

            return value;
        }
    }

    public sealed record AssetCreationData(string ModelName, string StatusName);
}
