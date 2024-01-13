using System.ComponentModel.DataAnnotations;
using Api.DataTransferModels;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CommandQuerryModels.CommandModels;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly CommentService _service;

    public CommentController(CommentService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("create")]
    [RequireAuthentication]
    public IActionResult CreateComment([FromBody] CreateCommentCommand command)
    {
        try
        {
            var comment = _service.CreateComment(command);
            return Ok(new ResponseDto { MessageToClient = "Comment created successfully", ResponseData = comment });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.ValidationResult.ErrorMessage });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpGet]
    [Route("/api/blog/{blogId}/comments")]
    public IActionResult GetAllComments(int blogId)
    {
        try
        {
            var comments = _service.GetAllComments(blogId);
            if (comments.Any())
            {
                return Ok(new ResponseDto { MessageToClient = "Successfully fetched", ResponseData = comments });
            }

            return NotFound(new ResponseDto { MessageToClient = "No comments available" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpGet]
    [Route("by-id/{id}")]
    [RequireAuthentication]
    public IActionResult GetCommentById(int id)
    {
        try
        {
            var comment = _service.GetCommentById(id);
            if (comment != null)
            {
                return Ok(new ResponseDto { MessageToClient = "Comment fetched successfully", ResponseData = comment });
            }

            return NotFound(new ResponseDto { MessageToClient = "Comment not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpPut]
    [Route("update")]
    [RequireAuthentication]
    public IActionResult UpdateComment([FromBody] UpdateCommentCommand command)
    {
        try
        {
            _service.UpdateComment(command);
            return Ok(new ResponseDto { MessageToClient = "Comment updated successfully" });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.ValidationResult.ErrorMessage });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpDelete]
    [Route("delete/{id}")]
    [RequireAuthentication]
    public IActionResult DeleteComment(int id)
    {
        try
        {
            int loggedUserId = HttpContext.GetSessionData().UserId;
            string loggedUserRole = HttpContext.GetSessionData().Role.ToString();

            _service.DeleteComment(id, loggedUserId, loggedUserRole);
            return Ok(new ResponseDto { MessageToClient = "Comment deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }
}