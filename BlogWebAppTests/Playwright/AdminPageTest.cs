namespace BlogWebAppTests.Playwright;

[TestFixture]
public class AdminPageTest : AuthenticatedPageTest
{
    [SetUp]
    public new async Task SetUp()
    {
        await base.SetUp();
        await Page.GotoAsync("http://localhost:4200/admin/admin-home");
    }
    
    [TearDown]
    public void Teardown()
    {
        Helper.TriggerRebuild();
    }
    
    [Test]
    public async Task ShouldListBlogsAndDeleteFirstBlog()
    {
        var blogPosts = Page.Locator("ion-card");
        await Expect(blogPosts).ToHaveCountAsync(3, new() { Timeout = 5000 });
        
        var firstBlogDeleteButton = Page.Locator("ion-card:first-of-type ion-button[color='danger']");
        await Expect(firstBlogDeleteButton).ToBeVisibleAsync();
        
        await firstBlogDeleteButton.ClickAsync();
        var confirmDialog = Page.Locator(".alert-wrapper");
        await Expect(confirmDialog).ToBeVisibleAsync(new() { Timeout = 5000 });
        
        var confirmButton = confirmDialog.Locator("text='Yes'");
        await confirmButton.ClickAsync();
        
        await Expect(confirmDialog).ToBeHiddenAsync();
        
        var addButton = Page.Locator("ion-fab-button");
        await addButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync("http://localhost:4200/admin/create-blog");
        
    }
}