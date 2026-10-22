using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Pages
{
    public sealed class AssetDetailsPage : BasePage
    {
        public AssetDetailsPage(IPage page) : base(page)
        {
        }

        public ILocator TextLocator(string text, ILocator? scope = null, bool exact = true)
        {
            return (scope ?? Page.Locator("body"))
                .GetByText(text, new() { Exact = exact });
        }

        public async Task OpenHistoryAsync()
        {
            await Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions
            {
                Name = "History"
            }).ClickAsync();
        }

        public ILocator GetHistoryTableRows()
        {
            var historyTableRows = Page.Locator("#assetHistoryListingTable tbody tr");
            return historyTableRows;
        }
    }
}
