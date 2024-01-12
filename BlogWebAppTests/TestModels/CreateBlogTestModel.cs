namespace BlogWebAppTests.TestModels;

public class CreateBlogTestModel
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int CategoryId { get; set; }
    public string? FeaturedImage { get; set; }
}