using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Service.CommandQuerryModels.CommandModels;

namespace Service;

public class CommentService
{
    private readonly ILogger<CommentService> _logger;
    private readonly ICommentRepository _commentRepository;

    public CommentService(ILogger<CommentService> logger, ICommentRepository commentRepository)
    {
        _logger = logger;
        _commentRepository = commentRepository;
    }

    public Comment CreateComment(CreateCommentCommand command)
    {
        try
        {
            return _commentRepository.Create(command.UserId, command.BlogId, command.Text);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating comment: {Message}", ex.Message);
            throw;
        }
    }

    public IEnumerable<Comment> GetAllComments(int blogId)
    {
        try
        {
            return _commentRepository.GetAllComments(blogId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all comments: {Message}", ex.Message);
            throw;
        }
    }

    public Comment? GetCommentById(int id)
    {
        try
        {
            return _commentRepository.GetById(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving comment by ID: {Message}", ex.Message);
            throw;
        }
    }

    public void UpdateComment(UpdateCommentCommand command)
    {
        try
        {
            _commentRepository.Update(command.Id, command.UserId, command.Text);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating comment: {Message}", ex.Message);
            throw;
        }
    }

    public void DeleteComment(int commentId, int requestingUserId, string requestingUserRole)
    {
        try
        {
            _commentRepository.Delete(commentId, requestingUserId, requestingUserRole);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting comment: {Message}", ex.Message);
            throw;
        }
    }
}