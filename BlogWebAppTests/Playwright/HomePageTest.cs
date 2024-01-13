using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace BlogWebAppTests.Playwright;

[TestFixture]
public class HomePageTest : PageTest
{
    [SetUp]
    public void SetUp()
    {
        Helper.TriggerRebuild();
    }
    
    [Test]
    public async Task HomepageHasExpectedTitle()
    {
        await Page.GotoAsync("http://localhost:4200/home");
        
        await Expect(Page).ToHaveTitleAsync(new Regex("Blog Stories"));
        
    }

    [Test]
    public async Task ToolBarHasExpectedText()
    {
        await Page.GotoAsync("http://localhost:4200/home");
        var toolBarTextLocator = Page.Locator("ion-title");
        
        await Expect(toolBarTextLocator).ToHaveTextAsync("Welcome To BlogStories");
    }

    [Test]
    public async Task ThreeBlogsShouldBeShown()
    {
        await Page.GotoAsync("http://localhost:4200/home");
        var blogPosts = Page.Locator("ion-card");
        await Expect(blogPosts).ToHaveCountAsync(3, new() { Timeout = 5000 });
    }

    [Test]
    public async Task ClickOnLogin()
    {
        await Page.GotoAsync("http://localhost:4200/home");
        var navigationTask = Page.WaitForNavigationAsync();
        
        var loginButton = Page.GetByRole(AriaRole.Link, new() { Name = "Login" });
        
        await Expect(loginButton).ToBeEnabledAsync();
        await loginButton.ClickAsync();
        
        await navigationTask;
        
        await Expect(Page).ToHaveURLAsync(new Regex("http://localhost:4200/account/login"));
    }
}