using SnipeITApp.SnipelITAppUIAutomation.Tests.Pages;
using SnipeITApp.SnipelITAppUIAutomation.Tests.Configuration;
using SnipeITApp.SnipelITAppUIAutomation.Tests.Fixtures;
using SnipeITApp.SnipelITAppUIAutomation.Tests.Workflows;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Tests
{
    [NonParallelizable]
    public sealed class AssetCreationTests : SnipeItPageTest
    {
        [SetUp]
        public Task SignInBeforeTestAsync()
        {
            return SignInAsync(Settings.Username, Settings.Password);
        }

        [Test]
        public async Task CreateAsset_CheckOutToRandomUser_AndVerifyHistory()
        {
            var assetWorkflow = new AssetWorkflow(Page, Settings);
            var testData = TestData.Load();
            var asset = await assetWorkflow.CreateAssetAsync(testData.AssetCreation);
            TestContext.Out.WriteLine($"Test asset tag: {asset.AssetTag}");
            TestContext.Out.WriteLine($"Assigned user during asset creation: {asset.AssignedUser}");

            var assetDetailsPage = await assetWorkflow.OpenAssetDetailsAsync(asset.AssetTag);
            await VerifyAssetDetailsAsync(
                assetDetailsPage,
                asset.AssetTag,
                asset.AssignedUser,
                testData.AssetCreation);

            await assetDetailsPage.OpenHistoryAsync();
            await VerifyAssetHistoryAsync(
                assetDetailsPage,
                asset.AssetTag,
                asset.AssignedUser,
                testData.AssetCreation);
        }

        private async Task VerifyAssetDetailsAsync(
            AssetDetailsPage assetDetailsPage,
            string assetTag,
            string assignedUser,
            AssetCreationData assetData)
        {
            var assetTagOnPage = assetDetailsPage.TextLocator(assetTag, exact: true);
            await assetTagOnPage.WaitForAsync();
            await Expect(assetTagOnPage).ToHaveCountAsync(1);

            var assignedUserOnPage = assetDetailsPage.TextLocator(
                assignedUser,
                Page.Locator($"a[title='{assignedUser}']"),
                exact: true);
            await assignedUserOnPage.WaitForAsync();
            await Expect(assignedUserOnPage).ToHaveCountAsync(1);

            var readyToDeployStatus = assetDetailsPage.TextLocator(
                assetData.StatusName,
                Page.Locator("div.well"),
                exact: false);
            await readyToDeployStatus.WaitForAsync();
            await Expect(readyToDeployStatus).ToHaveCountAsync(1);

            var modelOnPage = assetDetailsPage.TextLocator(
                assetData.ModelName,
                Page.Locator("li#model"),
                exact: false);
            await modelOnPage.WaitForAsync();
            await Expect(modelOnPage).ToHaveCountAsync(1);
        }

        private async Task VerifyAssetHistoryAsync(
            AssetDetailsPage assetDetailsPage,
            string assetTag,
            string assignedUser,
            AssetCreationData assetData)
        {
            var historyTableRows = assetDetailsPage.GetHistoryTableRows();
            await Expect(historyTableRows).ToHaveCountAsync(2);

            var firstRow = historyTableRows.Nth(0);
            await Expect(firstRow.Locator("td").Nth(3)).ToHaveTextAsync("create new");
            await Expect(firstRow.Locator("td").Nth(4)).ToContainTextAsync(assetTag);
            await Expect(firstRow.Locator("td").Nth(4)).ToContainTextAsync(
                assetData.ModelName,
                new() { IgnoreCase = true });

            var secondRow = historyTableRows.Nth(1);
            await Expect(secondRow.Locator("td").Nth(3)).ToHaveTextAsync("checkout");
            await Expect(secondRow.Locator("td").Nth(5)).ToContainTextAsync(assignedUser);
            await Expect(secondRow.Locator("td").Nth(9)).ToContainTextAsync("Checked out on asset creation");
        }

    }
}
