using SnipeITApp.SnipelITAppUIAutomation.Tests.Configuration;
using SnipeITApp.SnipelITAppUIAutomation.Tests.Pages;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Fixtures
{
    public abstract class SnipeItPageTest : PageTest
    {
        protected TestSettings Settings { get; } = TestSettings.Load();

        [SetUp]
        public Task SetUpSnipeItAsync()
        {
            Page.SetDefaultTimeout((float)Settings.Timeout.TotalMilliseconds);
            Page.SetDefaultNavigationTimeout((float)Settings.Timeout.TotalMilliseconds);

            return Task.CompletedTask;
        }

        protected async Task SignInAsync(string username, string password)
        {
            var loginPage = new LoginPage(Page, Settings.BaseUrl);
            await loginPage.NavigateAsync();
            await loginPage.SignInAsync(username, password);
        }
    }
}
