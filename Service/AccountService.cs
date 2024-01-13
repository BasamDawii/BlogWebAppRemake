using System.Security.Authentication;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Npgsql;
using Service.CommandQuerryModels.CommandModels;
using Service.PasswordService;

namespace Service;

public class AccountService
{
    private readonly ILogger<AccountService> _logger;
    private readonly IPasswordHashRepository _passwordHashRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountService(ILogger<AccountService> logger, IAccountRepository accountRepository,
        IPasswordHashRepository passwordHashRepository)
    {
        _logger = logger;
        _accountRepository = accountRepository;
        _passwordHashRepository = passwordHashRepository;
    }

    public User? Authenticate(UserLoginCommand command)
    {
        try
        {
            var passwordHash = _passwordHashRepository.GetByEmail(command.Email);
            var hashAlgorithm = PasswordHashAlgorithm.Create(passwordHash.Algorithm);
            var isValid = hashAlgorithm.VerifyHashedPassword(command.Password, passwordHash.Hash, passwordHash.Salt);
            if (!isValid)
            {
                _logger.LogWarning("Invalid credentials attempt for email: {Email}", command.Email);
                throw new InvalidCredentialException(
                    "Invalid credentials provided. Please check your email and password.");
            }

            return _accountRepository.GetById(passwordHash.UserId);
        }
        catch (InvalidCredentialException ex)
        {
            throw;
        }
        catch (NpgsqlException ex)
        {
            _logger.LogError(ex, "Database error during authentication for email: {Email}", command.Email);
            throw new AuthenticationException(
                "A database error occurred during authentication. Please try again later.");
        }
        catch (RepositoryException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during authentication for email: {Email}", command.Email);
            throw new ServiceException("An unexpected error occurred during authentication service. Please try again later.");
        }
    }

    public User Register(CreateUserCommand command)
    {
        try
        {
            var hashAlgorithm = PasswordHashAlgorithm.Create();
            var salt = hashAlgorithm.GenerateSalt();
            var hash = hashAlgorithm.HashPassword(command.Password, salt);
            var user = _accountRepository.Create(command.FullName, command.Email);
            _passwordHashRepository.Create(user.Id, hash, salt, hashAlgorithm.GetName());

            return user;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Registration failed: {Message}", ex.Message);
            throw;
        }
        catch (TimeoutException ex)
        {
            throw;
        }
        catch (RepositoryException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error during registration: {Message}", ex.Message);
            throw new ServiceException("Unexpected error during registration service", ex);
        }
    }  
    
    public User? Get(SessionData data)
    {
        try
        {
            return _accountRepository.GetById(data.UserId);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving user: {Message}", ex.Message);
            throw new Exception("An error occurred while retrieving user information.");
        }
    }
    
    public IEnumerable<User> GetAllUsers()
    {
        try
        {
            return _accountRepository.GetAllUsers();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error retrieving all users: {Message}", ex.Message);
            throw new ServiceException("An error occurred while retrieving all users.");
        }
    }
    
    public void DeleteUser(int userId)
    {
        try
        {
            _accountRepository.DeleteUser(userId);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogError("Error deleting user: {Message}", ex.Message);
            throw new ServiceException("Error occurred while deleting the user: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError("Unexpected error during deleting user: {Message}", ex.Message);
            throw new ServiceException("An unexpected error occurred during deleting the user.", ex);
        }
    }
}