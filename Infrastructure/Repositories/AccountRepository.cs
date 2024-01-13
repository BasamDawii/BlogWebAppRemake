using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _schema;

    public AccountRepository(NpgsqlDataSource dataSource, DatabaseConfig dbConfig)
    {
        _dataSource = dataSource;
        _schema = dbConfig.Schema;
    }
    
   public User Create(string fullName, string email)
    {
        var sql = $@"
INSERT INTO {_schema}.users (full_name, email)
VALUES (@fullName, @email)
RETURNING
    id as {nameof(User.Id)},
    full_name as {nameof(User.FullName)},
    email as {nameof(User.Email)};"; 
        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirst<User>(sql, new { fullName, email});
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            throw new InvalidOperationException("A user with this email address already exists.", ex);
        }
        catch (TimeoutException ex)
        {
            throw new TimeoutException("The operation timed out.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An unexpected error occurred while dealing with database.", ex);
        }

    }

    public User? GetById(int id)
    {
        var sql = $@"
SELECT
    id as {nameof(User.Id)},
    full_name as {nameof(User.FullName)},
    email as {nameof(User.Email)},
    role as {nameof(User.Role)}
FROM {_schema}.users
WHERE id = @id;
";
        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirstOrDefault<User>(sql, new { id });
        }
        catch (PostgresException ex)
        {
            throw new Exception("Database operation failed.", ex);
        }
        catch (Exception ex)
        {
            throw new RepositoryException("An unexpected error occurred while dealing with database.", ex);
        }
    }
    
public User? GetUserByEmail(string email)
{
    var sql = $@"
SELECT id, full_name, email, role
FROM {_schema}.users
WHERE email = @Email;";

    try
    {
        using var connection = _dataSource.OpenConnection();
        return connection.QueryFirstOrDefault<User>(sql, new { Email = email });
    }
    catch (PostgresException ex)
    {
        throw new Exception("Database operation failed.", ex);
    }
    catch (Exception ex)
    {
        throw new Exception("An unexpected error occurred while retrieving user by email.", ex);
    } 
}
    
public void DeleteUser(int id)
{
    var sql = $@"DELETE FROM {_schema}.users WHERE id = @Id;";
    using (var connection = _dataSource.OpenConnection())
        try
        {
            connection.Execute(sql, new { Id = id });
        }
        catch (NpgsqlException ex)
        {
            throw new InvalidOperationException("An error occurred while deleting the user.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred while deleting the user.", ex);
        } 
}

public IEnumerable<User> GetAllUsers()
{
    var sql = $@"
SELECT
    id as {nameof(User.Id)},
    full_name as {nameof(User.FullName)},
    email as {nameof(User.Email)},
    role as {nameof(User.Role)}
FROM {_schema}.users;";

    try
    {
        using var connection = _dataSource.OpenConnection();
        return connection.Query<User>(sql);
    }
    catch (PostgresException ex)
    {
        throw new Exception("Database operation failed.", ex);
    }
    catch (Exception ex)
    {
        throw new RepositoryException("An unexpected error occurred while dealing with database.", ex);
    }
}

}