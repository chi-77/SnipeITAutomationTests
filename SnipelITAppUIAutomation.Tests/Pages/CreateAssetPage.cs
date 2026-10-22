using Microsoft.Playwright;

namespace SnipeITApp.SnipelITAppUIAutomation.Tests.Pages
{
    public sealed class CreateAssetPage : NavigablePage
    {     
        public CreateAssetPage(IPage page, Uri baseUrl) : base(page, baseUrl)
        {
        }

        public Task NavigateAsync()
        {
            return NavigateToAsync("/hardware/create");
        }

        public async Task<string> GetAssetTagAsync()
        {
            var assetTagInput = Page.Locator("#asset_tag");
            var assetTag = await assetTagInput.InputValueAsync();
            if (string.IsNullOrWhiteSpace(assetTag))
            {
                throw new InvalidOperationException("The asset tag was not populated on the create asset page.");
            }

            return assetTag.Trim();
        }

        public async Task SelectModelAsync(string modelName)
        {
            await Page.Locator("#select2-model_select_id-container").ClickAsync();
            await Page.Locator(".select2-search__field").FillAsync(modelName);
            await Page.Locator(".select2-container--open .select2-results__option")
                       .Filter(new()
                       {
                           HasTextRegex = new Regex(
                               Regex.Escape(modelName),
                               RegexOptions.IgnoreCase)
                       })
                       .First
                       .ClickAsync();
        }

        public async Task SelectStatusAsync(string statusName)
        {
            await Page.Locator("#select2-status_select_id-container").ClickAsync();
            await Page.Locator(".select2-search__field").FillAsync(statusName);
            await Page.Locator(".select2-container--open .select2-results__option")
                       .Filter(new() { HasTextString = statusName })
                       .First
                       .ClickAsync();
        }

        public async Task<string> SelectRandomAssignedUserAsync()
        {
            await Page.Locator("#select2-assigned_user_select-container").ClickAsync();

            var options = Page.Locator(
                ".select2-container--open .select2-results__option[role='option']:not([aria-disabled='true'])");
            await options.First.WaitForAsync(
                new LocatorWaitForOptions { State = WaitForSelectorState.Visible });

            var optionCount = await options.CountAsync();
            if (optionCount == 0)
            {
                throw new InvalidOperationException("No users were available in the assigned-user dropdown.");
            }

            var selectedOption = options.Nth(Random.Shared.Next(optionCount));
            var rawLabel = await selectedOption.InnerTextAsync();
            var assignedUserLabel = Regex.Replace(rawLabel, @"\s*\([^)]*\).*$", "").Trim();
            
            await selectedOption.ClickAsync();
            return assignedUserLabel;
        }

        public async Task SaveAsync()
        {
            await Page.Locator("#submit_button").ClickAsync();

            var errorMessages = new List<string>();
            var errorAlert = Page.Locator("div.alert.alert-danger[role='alert']")
                .Filter(new() { HasTextString = "Please check the form below for errors" });

            if (await errorAlert.IsVisibleAsync())
            {
                errorMessages.Add((await errorAlert.InnerTextAsync()).Trim());
            }

            var requiredFieldAlerts = Page.Locator(".alert-msg[role='alert']:visible");
            if (await requiredFieldAlerts.CountAsync() > 0)
            {
                errorMessages.AddRange(await requiredFieldAlerts.AllTextContentsAsync());
            }

            if (errorMessages.Count > 0)
            {
                throw new InvalidOperationException(
                    $"Asset creation failed after saving: {string.Join("; ", errorMessages)}");
            }
        }
    }
}
