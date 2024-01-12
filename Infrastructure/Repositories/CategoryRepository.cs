using Dapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Npgsql;

namespace Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly string _schema;

    public CategoryRepository(NpgsqlDataSource dataSource, DatabaseConfig dbConfig)
    {
        _dataSource = dataSource;
        _schema = dbConfig.Schema;
    }
    
    
    public Category Create(string name)
    {
        var sql = $@"
INSERT INTO {_schema}.categories (name)
VALUES (@Name)
RETURNING id, name;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirst<Category>(sql, new { Name = name });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the category.", ex);
        }
    }
    
    public IEnumerable<Category> GetAllCategories()
    {
        var sql = $@"
SELECT * FROM {_schema}.categories;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.Query<Category>(sql);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving categories.", ex);
        }
    }
    
    public Category? GetById(int id)
    {
        var sql = $@"
SELECT id, name
FROM {_schema}.categories
WHERE id = @Id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            return connection.QueryFirstOrDefault<Category>(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while fetching the category.", ex);
        }
    }
    
    public void Update(int id, string newName)
    {
        var sql = $@"
UPDATE {_schema}.categories
SET name = @NewName
WHERE id = @Id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { Id = id, NewName = newName });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the category.", ex);
        }
    }
    
    public void Delete(int id)
    {
        var sql = $@"
DELETE FROM {_schema}.categories
WHERE id = @Id;";

        try
        {
            using var connection = _dataSource.OpenConnection();
            connection.Execute(sql, new { Id = id });
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while deleting the category.", ex);
        }
    }
}