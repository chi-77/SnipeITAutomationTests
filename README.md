# SnipeITAutomationTests

UI automation tests for Snipe-IT using .NET 10, NUnit, and Microsoft Playwright.

## Prerequisites

- Visual Studio 2026 with the .NET 10 SDK, or the .NET 10 SDK installed separately.
- Access to the Snipe-IT instance under test.
- A Snipe-IT test account that can create assets, check assets out, and view asset details and history.

## Local setup

Run the following steps from the repository root:

```powershell
cd C:\personal\AutomationTests
dotnet restore .\SnipeITApp.csproj
dotnet build .\SnipeITApp.csproj
```

Install the Playwright browser binaries after the project has been built:

```powershell
.\bin\Debug\net10.0\playwright.ps1 install
```

## Configure local test settings

The repository uses two local files when running tests in headed mode:

- `SnipelITAppUIAutomation.Tests\appsettings.json` contains the Snipe-IT URL, test credentials, and timeout.
- `local.runsettings` sets `HEADED=1`, which makes Playwright display the browser.

`appsettings.json` is intentionally ignored by Git. Before running the tests, open that file and replace the placeholder values with credentials for the local Snipe-IT test account:

```json
{
  "SNIPEIT_BASE_URL": "https://demo.snipeitapp.com",
  "SNIPEIT_USERNAME": "your-test-username",
  "SNIPEIT_PASSWORD": "your-test-password",
  "SNIPEIT_TIMEOUT_SECONDS": "30"
}
```

If `appsettings.json` is not present, create it from the committed example:

```powershell
Copy-Item .\SnipelITAppUIAutomation.Tests\appsettings.example.json .\SnipelITAppUIAutomation.Tests\appsettings.json
```

Do not commit `appsettings.json` or real credentials. The application also supports .NET User Secrets and environment variables, which override values from `appsettings.json`, but they are optional for the documented local setup.

## Run headed tests from PowerShell

Use the provided `local.runsettings` file:

```powershell
dotnet test .\SnipeITApp.csproj --settings .\local.runsettings
```

The `local.runsettings` file configures Playwright to run with a visible browser. No test-code changes are required.

## Run headed tests from Visual Studio

1. Open `SnipeIT.slnx` in Visual Studio.
2. Ensure `SnipelITAppUIAutomation.Tests\appsettings.json` contains the correct local URL and credentials.
3. Open **Test Explorer**.
4. Select **Test** > **Configure Run Settings** > **Select Test Settings File**.
5. Select the repository's `local.runsettings` file.
6. Run `CreateAsset_CheckOutToRandomUser_AndVerifyHistory`, or run all discovered tests.

The selected `local.runsettings` file sets `HEADED=1`, so the browser should be visible during execution.

## Test data

Asset creation values are stored in:

```text
SnipelITAppUIAutomation.Tests\Data\testdata.json
```

The configured Snipe-IT instance must contain the referenced model and status, such as `MacBook Pro 13\"` and `Ready to Deploy`.

## Project test command

To run the test suite from the repository root, use:

```powershell
dotnet test .\SnipeITApp.csproj --settings .\local.runsettings
