using Microsoft.Playwright;
using SnipeITApp.SnipelITAppUIAutomation.Tests.Configuration;
using SnipeITApp.SnipelITAppUIAutomation.Tests.Pages;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Workflows
{
    public sealed class AssetWorkflow
    {
        private readonly IPage _page;
        private readonly TestSettings _settings;

        public AssetWorkflow(IPage page, TestSettings settings)
        {
            _page = page;
            _settings = settings;
        }

        public async Task<AssetCreationResult> CreateAssetAsync(AssetCreationData assetData)
        {
            var createAssetPage = new CreateAssetPage(_page, _settings.BaseUrl);
            await createAssetPage.NavigateAsync();

            var assetTag = await createAssetPage.GetAssetTagAsync();
            await createAssetPage.SelectModelAsync(assetData.ModelName);
            await createAssetPage.SelectStatusAsync(assetData.StatusName);
            var assignedUser = await createAssetPage.SelectRandomAssignedUserAsync();

            await createAssetPage.SaveAsync();

            return new AssetCreationResult(assetTag, assignedUser);
        }

        public async Task<AssetDetailsPage> OpenAssetDetailsAsync(string assetTag)
        {
            var assetsListPage = new AssetsListPage(_page, _settings.BaseUrl);
            await assetsListPage.NavigateAsync();
            await assetsListPage.SearchByAssetTagAsync(assetTag);

            var assetRow = assetsListPage.GetAssetRow(assetTag);
            await assetRow.WaitForAsync();
            await assetsListPage.OpenAssetAsync(assetRow, assetTag);

            return new AssetDetailsPage(_page);
        }
    }

    public sealed record AssetCreationResult(string AssetTag, string AssignedUser);
}
