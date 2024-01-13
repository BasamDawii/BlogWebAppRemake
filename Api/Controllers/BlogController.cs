using System.ComponentModel.DataAnnotations;
using Api.DataTransferModels;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CommandQuerryModels.CommandModels;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly BlogService _service;

    public BlogController(BlogService service)
    {
        _service = service;
    }
    
    [HttpPost]
    [Route("create")]
    [RequireAuthentication]
    public IActionResult CreateBlog([FromBody] CreateBlogCommand command)
    {
        try
        {
            var blog = _service.CreateBlog(command);
            return Ok(new ResponseDto { MessageToClient = "Blog created successfully", ResponseData = blog });
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
    [Route("blogs")]
    public IActionResult GetAllBlogs()
    {
        try
        {
            var blogs = _service.GetAllBlogs();
            if (blogs.Any())
            {
                return Ok(new ResponseDto { MessageToClient = "Successfully fetched", ResponseData = blogs });
            }

            return NotFound(new ResponseDto { MessageToClient = "No blogs available" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpGet]
    [Route("blogs/{id}")]
    [RequireAuthentication]
    public IActionResult GetBlogById(int id)
    {
        try
        {
            var blog = _service.GetBlogById(id);
            if (blog != null)
            {
                return Ok(new ResponseDto { MessageToClient = "Blog fetched successfully", ResponseData = blog });
            }

            return NotFound(new ResponseDto { MessageToClient = "Blog not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpPut]
    [Route("update")]
    [RequireAuthentication]
    public IActionResult UpdateBlog([FromBody] UpdateBlogCommand command)
    {
        try
        {
            _service.UpdateBlog(command);
            return Ok(new ResponseDto { MessageToClient = "Blog updated successfully" });
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
    [Route("delete-by-admin/{id}")]
    [RequireAdminAuthentication]
    public IActionResult DeleteBlogByAdmin(int id)
    {
        try
        {
            _service.DeleteBlogByAdmin(id);
            return Ok(new ResponseDto { MessageToClient = "Blog deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }
    
    [HttpDelete]
    [Route("delete-by-user/{id}")]
    [RequireAuthentication]
    public IActionResult DeleteBlogByUser(int id)
    {
        try
        {
            int loggedUserId = HttpContext.GetSessionData().UserId;
            _service.DeleteBlogByUser(id, loggedUserId);
            return Ok(new ResponseDto { MessageToClient = "Blog deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }

    [HttpGet]
    [Route("blogs/user/{userId}")]
    [RequireAuthentication]
    public IActionResult GetBlogsByUserId(int userId)
    {
        try
        {
            var blogs = _service.GetBlogsByUserId(userId);
            if (blogs.Any())
            {
                return Ok(new ResponseDto { MessageToClient = "Blogs fetched successfully", ResponseData = blogs });
            }

            return NotFound(new ResponseDto { MessageToClient = "No blogs found for the user" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }
}