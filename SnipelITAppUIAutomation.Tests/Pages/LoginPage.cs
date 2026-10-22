using Microsoft.Playwright;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Pages
{
    public sealed class LoginPage : NavigablePage
    {
        public LoginPage(IPage page, Uri baseUrl) : base(page, baseUrl)
        {           
        }

        public Task NavigateAsync()
        {
            return NavigateToAsync("/login");
        }

        public async Task SignInAsync(string username, string password)
        {
            await Page.Locator("input[name='username']").FillAsync(username);
            await Page.Locator("input[name='password']").FillAsync(password);

            var navigation = Page.WaitForURLAsync(
                url => !url.Contains("/login", StringComparison.OrdinalIgnoreCase));
            await Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions
            {
                Name = "Login",
                Exact = true
            }).ClickAsync();
            await navigation;
        }
    }
}
