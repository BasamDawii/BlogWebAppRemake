using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Service.CommandQuerryModels.CommandModels;

namespace Service;

public class CategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ILogger<CategoryService> logger, ICategoryRepository categoryRepository)
    {
        _logger = logger;
        _categoryRepository = categoryRepository;
    }

    public Category CreateCategory(CreateCategoryCommand command)
    {
        try
        {
            return _categoryRepository.Create(command.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error creating category: {Message}", ex.Message);
            throw;
        }
    }

    public IEnumerable<Category> GetAllCategories()
    {
        try
        {
            return _categoryRepository.GetAllCategories();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all categories: {Message}", ex.Message);
            throw;
        }
    }

    public Category? GetCategoryById(int id)
    {
        try
        {
            return _categoryRepository.GetById(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving category by ID: {Message}", ex.Message);
            throw;
        }
    }

    public void UpdateCategory(UpdateCategoryCommand command)
    {
        try
        {
            _categoryRepository.Update(command.Id, command.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error updating category: {Message}", ex.Message);
            throw;
        }
    }

    public void DeleteCategory(int id)
    {
        try
        {
            _categoryRepository.Delete(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error deleting category: {Message}", ex.Message);
            throw;
        }
    }
}