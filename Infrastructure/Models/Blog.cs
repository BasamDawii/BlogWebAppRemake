namespace Infrastructure.Models;

public class Blog
{
    
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublicationDate { get; set; }
    public int CategoryId { get; set; }
    public string FeaturedImage { get; set; }
    public string UserFullName { get; set; }
    public int CommentCount { get; set; }
    
}