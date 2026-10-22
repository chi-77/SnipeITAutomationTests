using Microsoft.Playwright;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Pages
{
    public sealed class AssetsListPage : NavigablePage
    {
        public AssetsListPage(IPage page, Uri baseUrl) : base(page, baseUrl)
        {
        }

        public Task NavigateAsync()
        {
            return NavigateToAsync("/hardware");
        }

        public async Task SearchByAssetTagAsync(string assetTag)
        {
            var searchBox = Page.GetByRole(AriaRole.Searchbox, new() { Name = "Search" });
            await searchBox.FillAsync(assetTag);
            await searchBox.PressAsync("Enter");
        }

        public ILocator GetAssetRow(string assetTag)
        {
            return Page.GetByRole(AriaRole.Row).Filter(new() { HasText = assetTag });
        }

        public async Task OpenAssetAsync(string assetTag)
        {
            var assetRow = GetAssetRow(assetTag);
            await assetRow.GetByRole(AriaRole.Link, new() { Name = assetTag, Exact = true }).ClickAsync();
        }
    }
}
