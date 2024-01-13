using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IBlogRepository
{
    Blog Create(int userId, string title, string content, int categoryId, string imgUrl);
    IEnumerable<Blog> GetAllBlogs();
    IEnumerable<Blog> GetBlogsByUserId(int userId);
    Blog? GetById(int id);

    void Update(int id, int userId, string title, string content, int categoryId, string imgUrl);

    void DeleteByUser(int id, int userId);
    void DeleteByAdmin(int id);

}