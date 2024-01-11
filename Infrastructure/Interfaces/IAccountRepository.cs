using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IAccountRepository
{
    User Create(string fullName, string email);
    User? GetById(int id);
    User? GetUserByEmail(string email);
    void DeleteUser(int id);
    IEnumerable<User> GetAllUsers();
}