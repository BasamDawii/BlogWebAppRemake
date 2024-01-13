using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _schema;

    public CommentRepository(NpgsqlDataSource dataSource, DatabaseConfig dbConfig)
    {
        _dataSource = dataSource;
        _schema = dbConfig.Schema;
    }

    public Comment Create(int userId, int blogId, string text)
    {
        var sql = $@"
INSERT INTO {_schema}.comments (user_id, blog_id, text, publication_date)
VALUES (@UserId, @BlogId, @Text, @PublicationDate)
RETURNING id as {nameof(Comment.Id)}, user_id as {nameof(Comment.UserId)}, 
blog_id as {nameof(Comment.BlogId)}, text as {nameof(Comment.Text)}, publication_date as {nameof(Comment.PublicationDate)};";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirst<Comment>(sql, new
            {
                UserId = userId,
                BlogId = blogId,
                Text = text,
                PublicationDate = DateTime.UtcNow
            });
        }
        
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the comment.", ex);
        }
    }

    public IEnumerable<Comment> GetAllComments(int blogId)
    {
        var sql = $@"
SELECT c.id as {nameof(Comment.Id)}, c.user_id as {nameof(Comment.UserId)}, 
c.blog_id as {nameof(Comment.BlogId)}, c.text as {nameof(Comment.Text)}, 
c.publication_date as {nameof(Comment.PublicationDate)}, u.full_name as {nameof(Comment.UserFullName)}
FROM {_schema}.comments c
INNER JOIN {_schema}.users u ON c.user_id = u.id
WHERE c.blog_id = @BlogId;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.Query<Comment>(sql, new { BlogId = blogId });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving comments.", ex);
        }
    }


    public Comment? GetById(int id)
    {
        var sql = $@"
SELECT id as {nameof(Comment.Id)}, user_id as {nameof(Comment.UserId)}, 
blog_id as {nameof(Comment.BlogId)}, text as {nameof(Comment.Text)}, publication_date as {nameof(Comment.PublicationDate)} FROM {_schema}.comments
WHERE id = @Id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirstOrDefault<Comment>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching the comment.", ex);
        }
    }

    public void Update(int id, int userId, string newText)
    {
        var sql = $@"
UPDATE {_schema}.comments
SET text = @NewText, publication_date = @PublicationDate
WHERE id = @Id AND user_id = @UserId;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new
            {
                NewText = newText,
                PublicationDate = DateTime.UtcNow,
                UserId = userId,
                Id = id
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the comment.", ex);
        }
    }

    public void Delete(int commentId, int requestingUserId, string requestingUserRole)
    {
        var sql = $@"
DELETE FROM {_schema}.comments
WHERE id = @CommentId AND 
(user_id = @RequestingUserId OR 
@RequestingUserRole = 'Admin' OR 
@RequestingUserId IN (SELECT user_id FROM {_schema}.blogs WHERE id = (SELECT blog_id FROM {_schema}.comments WHERE id = @CommentId)));";

        try
        {
            using var connection = _dataSource.OpenConnection();
            int affectedRows = connection.Execute(sql, new 
            { 
                CommentId = commentId, 
                RequestingUserId = requestingUserId, 
                RequestingUserRole = requestingUserRole 
            });

            if (affectedRows == 0)
            {
                throw new Exception("No permission to delete this comment or comment does not exist.");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the comment.", ex);
        }
    }

}