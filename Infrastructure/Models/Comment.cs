namespace Infrastructure.Models;

public class Comment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BlogId { get; set; }
    public string Text { get; set; }
    public DateTime PublicationDate { get; set; }
    public string UserFullName { get; set; }
}