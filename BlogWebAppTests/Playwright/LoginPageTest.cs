using Microsoft.Playwright.NUnit;

namespace BlogWebAppTests.Playwright;

[TestFixture]
public class LoginPageTest : PageTest
{
    [SetUp]
    public void SetUp()
    {
        Helper.TriggerRebuild();
    }
    
    [Test]
    public async Task LoginFieldsAndButtonTest()
    {
        await Page.GotoAsync("http://localhost:4200/account/login");
        
        var emailFieldLocator = Page.Locator("ion-input[type='email']");
        var passwordFieldLocator = Page.Locator("ion-input[type='password']");
        
        await Expect(emailFieldLocator).ToBeVisibleAsync();
        await Expect(passwordFieldLocator).ToBeVisibleAsync();
        
        var loginButtonLocator = Page.Locator("ion-button[type='submit']");
        
        await Expect(loginButtonLocator).ToBeVisibleAsync();
    }
    
    [Test]
    public async Task AdminLoginAndRedirectToAdminHomeTest()
    {
        await Page.GotoAsync("http://localhost:4200/account/login");
        
        await Page.FillAsync("ion-input[type='email'] input", "blog-team@example.com");
        await Page.FillAsync("ion-input[type='password'] input", "password");
        
        await Page.ClickAsync("ion-button[type='submit']");
        
        await Page.WaitForNavigationAsync();
        
        var dashboardUrl = "http://localhost:4200/admin/admin-home";
        await Expect(Page).ToHaveURLAsync(dashboardUrl);
    }
}