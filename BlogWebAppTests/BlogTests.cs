using System.Net;
using System.Net.Http.Json;
using BlogWebAppTests.TestModels;
using FluentAssertions;
using Newtonsoft.Json;

namespace BlogWebAppTests;

public class BlogTests
{
    private HttpClient _httpClient;
    private string _baseAddress = "http://localhost:5000/api/blog/";

    [SetUp]
    public async Task Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [TearDown]
    public void Teardown()
    {
        Helper.TriggerRebuild();
    }
    
    [TestCase(1, "Tech Blog Test", "This is the first test blog post.", 1, "https://business.leeds.ac.uk/images/Disrupting_Tech_shutterstock_566877226_800_x_400.jpg")]
    [TestCase(1, "Health Blog Test", "This is the second test blog post.", 2, "https://www.statnews.com/wp-content/uploads/2022/03/AdobeStock_246942922.jpeg")]
    [TestCase(1, "Travel Blog Test", "This is the third test blog post.", 3, "https://www.candorblog.com/wp-content/uploads/2017/05/travel-022.jpg")]
    public async Task CreateBlog_ShouldFail_DueToAuthorization(int userId, string title, string content, int categoryId, string featuredImage)
    {
        // Arrange
        var createBlogCommand = new CreateBlogTestModel()
        {
            UserId = userId,
            Title = title,
            Content = content,
            CategoryId = categoryId,
            FeaturedImage = featuredImage
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync(_baseAddress + "create", createBlogCommand);
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseTestModel>(responseBody);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        responseObject?.MessageToClient.Should().Be("System error.");
        responseObject?.ResponseData.Should().BeNull();
    }
    
    [Test]
    public async Task GetAllBlogs_ShouldReturnSuccess_WithData()
    {
        // Act
        var response = await _httpClient.GetAsync(_baseAddress + "blogs");
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseTestModel>(responseBody);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        responseObject?.MessageToClient.Should().Be("Successfully fetched");
        responseObject?.ResponseData.Should().NotBeNull();
    }
}