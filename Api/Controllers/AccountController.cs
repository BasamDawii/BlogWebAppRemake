using System.Security.Authentication;
using Api.DataTransferModels;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CommandQuerryModels.CommandModels;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _service;

    public AccountController(AccountService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] UserLoginCommand command)
    {
        try
        {
            var user = _service.Authenticate(command);
            if (user != null)
            {
                HttpContext.SetSessionData(SessionData.FromUser(user));
                return Ok(new ResponseDto
                {
                    MessageToClient = "Successfully authenticated",
                    ResponseData = new { Role = user.Role }
                });
            }

            return Unauthorized(new ResponseDto { MessageToClient = "Invalid credentials." });
        }
        catch (AuthenticationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "Authentication failed." });
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new ResponseDto { MessageToClient = "You have just logged out..." });
    }
    
    [HttpPost("register")]
    public IActionResult Register([FromBody] CreateUserCommand command)
    {
        try
        {
            var user = _service.Register(command);
            return CreatedAtAction(nameof(WhoAmI), new { id = user.Id }, new ResponseDto
            {
                MessageToClient = "Registration successful."
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = ex.Message });
        }
        
        catch (ServiceException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ResponseDto { MessageToClient = ex.Message });
        }
        
    }
    
    [RequireAuthentication]
    [HttpGet("whoami")]
    public IActionResult WhoAmI()
    {
        try
        {
            var data = HttpContext.GetSessionData();
            var user = _service.Get(data);
            if (user != null)
            {
                return Ok(new ResponseDto
                {
                    ResponseData = user
                });
            }
            return NotFound(new ResponseDto { MessageToClient = "User not found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred." });
        }
    }
    
    [HttpGet("all-users")]
    [RequireAdminAuthentication]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _service.GetAllUsers();
            return Ok(new ResponseDto { ResponseData = users });
        }
        catch (ServiceException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred." });
        }
    }

    [HttpDelete("delete-user/{userId}")]
    [RequireAdminAuthentication]
    public IActionResult DeleteUser(int userId)
    {
        try
        {
            _service.DeleteUser(userId);
            return Ok(new ResponseDto { MessageToClient = "User deleted successfully." });
        }
        catch (ServiceException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseDto { MessageToClient = "An error occurred while deleting the user." });
        }
    }

}