using Microsoft.Playwright;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Pages
{
    public abstract class NavigablePage : BasePage
    {
        protected Uri BaseUrl { get; }

        protected NavigablePage(IPage page, Uri baseUrl)
            : base(page)
        {
            BaseUrl = baseUrl;
        }

        protected Task NavigateToAsync(string path)
        {
            return Page.GotoAsync(
                new Uri(BaseUrl, path).ToString());
        }   
    }
}
