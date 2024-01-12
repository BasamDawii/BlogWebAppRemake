using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Service.CommandQuerryModels.CommandModels;

namespace Service;

public class BlogService
{
    private readonly ILogger<BlogService> _logger;
    private readonly IBlogRepository _blogRepository;

    public BlogService(ILogger<BlogService> logger, IBlogRepository blogRepository)
    {
        _logger = logger;
        _blogRepository = blogRepository;
    }

    public Blog CreateBlog(CreateBlogCommand command)
    {
        try
        {
            return _blogRepository.Create(command.UserId, command.Title, command.Content, command.CategoryId, command.FeaturedImage);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating blog: {Message}", ex.Message);
            throw;
        }
    }

    public IEnumerable<Blog> GetAllBlogs()
    {
        try
        {
            return _blogRepository.GetAllBlogs();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all blogs: {Message}", ex.Message);
            throw;
        }
    }

    public Blog? GetBlogById(int id)
    {
        try
        {
            return _blogRepository.GetById(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving blog by ID: {Message}", ex.Message);
            throw;
        }
    }

    public void UpdateBlog(UpdateBlogCommand command)
    {
        try
        {
            _blogRepository.Update(command.Id, command.UserId, command.Title, command.Content, command.CategoryId, command.FeaturedImage);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating blog: {Message}", ex.Message);
            throw;
        }
    }

    public void DeleteBlogByAdmin(int id)
    {
        try
        {
            _blogRepository.DeleteByAdmin(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting blog by admin: {Message}", ex.Message);
            throw;
        }
    }

    public void DeleteBlogByUser(int id, int userId)
    {
        try
        {
            _blogRepository.DeleteByUser(id, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting blog by user: {Message}", ex.Message);
            throw;
        }
    }

    public IEnumerable<Blog> GetBlogsByUserId(int userId)
    {
        try
        {
            return _blogRepository.GetBlogsByUserId(userId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving blogs by user ID: {Message}", ex.Message);
            throw;
        }
    }
}