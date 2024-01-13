using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ICommentRepository
{
    Comment Create(int userId, int blogId, string text);
    IEnumerable<Comment> GetAllComments(int blogId);
    Comment? GetById(int id);
    void Update(int id, int userId, string newText);
    void Delete(int commentId, int requestingUserId, string requestingUserRole);
}