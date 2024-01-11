using Infrastructure.Models;

namespace Infrastructure.Interfaces;

public interface IPasswordHashRepository
{
    PasswordHash GetByEmail(string email);
    void Create(int userId, string hash, string salt, string algorithm);
}