using Dapper;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int> CreateUserAsync(User user)
    {
        var query = @"
        INSERT INTO Users
        (
            Name,
            Email,
            PasswordHash,
            Role,
            CreatedAt
        )
        VALUES
        (
            @Name,
            @Email,
            @PasswordHash,
            @Role,
            @CreatedAt
        )
        RETURNING Id";

        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            query,
            user);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var query =
        @"SELECT * FROM Users
          WHERE Email = @Email";

        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            query,
            new { Email = email });
    }
    public async Task<bool> UpdateUserAsync(User user)
    {
        var query = @"
            UPDATE Users
            SET
                Name = @Name,
                Email = @Email,
                Role = @Role,
                UpdatedAt=@UpdatedAt
            WHERE Id = @Id";

        using var connection = _context.CreateConnection();

        var rows =
            await connection.ExecuteAsync(query, user);

        return rows > 0;
    }
    public async Task<bool> DeleteUserAsync(int id)
    {
        var query =
            "DELETE FROM Users WHERE Id = @Id";

        using var connection = _context.CreateConnection();

        var rows =
            await connection.ExecuteAsync(
                query,
                new { Id = id });

        return rows > 0;
    }
    public async Task<User?> GetUserByIdAsync(int id)
    {
        var query =
            "SELECT * FROM Users WHERE Id = @Id";

        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(
            query,
            new { Id = id });
    }
    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        var query = "SELECT * FROM Users";
        
        using var connection = _context.CreateConnection();

        return await connection.QueryAsync<User>(query);
    }
}