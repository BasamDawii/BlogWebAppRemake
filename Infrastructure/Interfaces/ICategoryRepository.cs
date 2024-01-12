using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface ICategoryRepository
{
    Category Create(string name);
    IEnumerable<Category> GetAllCategories();
    Category? GetById(int id);
    void Update(int id, string newName);
    void Delete(int id);
}