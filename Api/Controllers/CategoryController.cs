using System.ComponentModel.DataAnnotations;
using Api.DataTransferModels;
using Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.CommandQuerryModels.CommandModels;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly CategoryService _service;

    public CategoryController(CategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("create")]
    [RequireAdminAuthentication]
    public IActionResult CreateCategory([FromBody] CreateCategoryCommand command)
    {
        try
        {
            var category = _service.CreateCategory(command);
            return Ok(new ResponseDto { MessageToClient = "Category created successfully", ResponseData = category });
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
    [Route("categories")]
    [RequireAuthentication]
    public IActionResult GetAllCategories()
    {
        try
        {
            var categories = _service.GetAllCategories();
            if (categories.Count() != 0)
            {
                return Ok(new ResponseDto() { MessageToClient = "Successfully fetched", ResponseData = categories });
            }

            return NotFound(new ResponseDto() { MessageToClient = "No available categories" });

        }
        
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = e.Message });
        }
    }
    [HttpPut]
    [Route("update")]
    [RequireAdminAuthentication]
    public IActionResult UpdateCategory([FromBody] UpdateCategoryCommand command)
    {
        try
        {
            _service.UpdateCategory(command);
            return Ok(new ResponseDto { MessageToClient = "Category updated successfully" });
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
    [RequireAdminAuthentication]
    public IActionResult DeleteCategory(int id)
    {
        try
        {
            _service.DeleteCategory(id);
            return Ok(new ResponseDto { MessageToClient = "Category deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }
    
    [HttpGet]
    [Route("categories/{id}")]
    [RequireAuthentication]
    public IActionResult GetCategoryById(int id)
    {
        try
        {
            var category = _service.GetCategoryById(id);
            if (category != null)
            {
                return Ok(new ResponseDto { MessageToClient = "Category fetched successfully", ResponseData = category });
            }

            return NotFound(new ResponseDto { MessageToClient = "Category not found" });
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto { MessageToClient = ex.Message });
        }
    }
}