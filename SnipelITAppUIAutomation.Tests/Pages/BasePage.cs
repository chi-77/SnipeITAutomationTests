using Microsoft.Playwright;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Pages
{
    public abstract class BasePage
    {
        protected IPage Page { get; }

        protected BasePage(IPage page)
        {
            Page = page;
        }
    }
}