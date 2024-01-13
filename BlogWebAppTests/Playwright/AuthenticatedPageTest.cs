using Microsoft.Playwright.NUnit;

namespace BlogWebAppTests.Playwright;

public class AuthenticatedPageTest : PageTest
{
    protected async Task LoginAsAdminAsync()
    {
        await Page.GotoAsync("http://localhost:4200/account/login");
        await Page.FillAsync("ion-input[type='email'] input", "blog-team@example.com");
        await Page.FillAsync("ion-input[type='password'] input", "password");
        await Page.ClickAsync("ion-button[type='submit']");
        await Page.WaitForNavigationAsync();
        
    }

    [SetUp]
    public async Task SetUp()
    {
        Helper.TriggerRebuild();
        await LoginAsAdminAsync();
    }
}