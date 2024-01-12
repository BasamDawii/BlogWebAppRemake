using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class BlogRepository : IBlogRepository
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _schema;

    public BlogRepository(NpgsqlDataSource dataSource, DatabaseConfig dbConfig)
    {
        _dataSource = dataSource;
        _schema = dbConfig.Schema;
    }

    public Blog Create(int userId, string title, string content, int categoryId, string imgUrl)
    {
        var sql = $@"
INSERT INTO {_schema}.blogs (user_id, title, content, publication_date, category_id, featured_image)
VALUES (@UserId, @Title, @Content, @PublicationDate, @CategoryId, @FeaturedImage)
RETURNING id as {nameof(Blog.Id)}, user_id as {nameof(Blog.UserId)}, title as {nameof(Blog.Title)}, 
content as {nameof(Blog.Content)}, publication_date as {nameof(Blog.PublicationDate)}, 
category_id as {nameof(Blog.CategoryId)}, featured_image as {nameof(Blog.FeaturedImage)};";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirst<Blog>(sql, new
            {
                UserId = userId,
                Title = title,
                Content = content,
                PublicationDate = DateTime.UtcNow,
                CategoryId = categoryId,
                FeaturedImage = imgUrl
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the blog.", ex);
        }
    }

    public IEnumerable<Blog> GetAllBlogs()
    {
        var sql = $@"
SELECT b.id as {nameof(Blog.Id)}, b.user_id as {nameof(Blog.UserId)}, b.title as {nameof(Blog.Title)}, 
b.content as {nameof(Blog.Content)}, b.publication_date as {nameof(Blog.PublicationDate)}, 
b.category_id as {nameof(Blog.CategoryId)}, b.featured_image as {nameof(Blog.FeaturedImage)},
u.full_name as {nameof(Blog.UserFullName)},
COUNT(c.id) as {nameof(Blog.CommentCount)}
FROM {_schema}.blogs b
JOIN {_schema}.users u ON b.user_id = u.id
LEFT JOIN {_schema}.comments c ON b.id = c.blog_id
GROUP BY b.id, u.id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.Query<Blog>(sql);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving blogs.", ex);
        }
    }



    public Blog? GetById(int id)
    {
        var sql = $@"
SELECT id as {nameof(Blog.Id)}, user_id as {nameof(Blog.UserId)}, title as {nameof(Blog.Title)}, 
content as {nameof(Blog.Content)}, publication_date as {nameof(Blog.PublicationDate)}, 
category_id as {nameof(Blog.CategoryId)}, featured_image as {nameof(Blog.FeaturedImage)}
FROM {_schema}.blogs
WHERE id = @Id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirstOrDefault<Blog>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching the blog.", ex);
        }
    }

    public void Update(int id, int userId, string title, string content, int categoryId, string imgUrl)
    {
        var sql = $@"
UPDATE {_schema}.blogs
SET title = @Title, content = @Content, publication_date = @PublicationDate, category_id = @CategoryId, featured_image = @FeaturedImage
WHERE id = @Id AND user_id = @UserId;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new
            {
                Title = title,
                Content = content,
                PublicationDate = DateTime.UtcNow,
                CategoryId = categoryId,
                FeaturedImage = imgUrl,
                UserId = userId,
                Id = id
            });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the blog.", ex);
        }
    }

    public void DeleteByAdmin(int id)
    {
        var sql = $@"
DELETE FROM {_schema}.blogs
WHERE id = @Id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the blog.", ex);
        }
    }
    
    public void DeleteByUser(int id, int userId)
    {
        var sql = $@"
DELETE FROM {_schema}.blogs
WHERE id = @Id AND user_id = @UserId;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { Id = id, UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the blog.", ex);
        }
    }
    
    public IEnumerable<Blog> GetBlogsByUserId(int userId)
    {
        var sql = $@"
SELECT b.id as {nameof(Blog.Id)}, b.user_id as {nameof(Blog.UserId)}, b.title as {nameof(Blog.Title)}, 
b.content as {nameof(Blog.Content)}, b.publication_date as {nameof(Blog.PublicationDate)}, 
b.category_id as {nameof(Blog.CategoryId)}, b.featured_image as {nameof(Blog.FeaturedImage)},
u.full_name as {nameof(Blog.UserFullName)},
COUNT(c.id) as {nameof(Blog.CommentCount)}
FROM {_schema}.blogs b
JOIN {_schema}.users u ON b.user_id = u.id
LEFT JOIN {_schema}.comments c ON b.id = c.blog_id
WHERE b.user_id = @UserId
GROUP BY b.id, u.full_name;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.Query<Blog>(sql, new { UserId = userId });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving blogs for the specified user.", ex);
        }
    }


}