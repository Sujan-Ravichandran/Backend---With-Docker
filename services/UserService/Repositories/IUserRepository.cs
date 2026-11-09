using UserService.Models;

namespace UserService.Repositories;

public interface IUserRepository
{
    Task<int> CreateUserAsync(User user);

    Task<User?> GetUserByEmailAsync(string email);

    Task<bool> DeleteUserAsync(int id);

    Task<bool> UpdateUserAsync(User user);

    Task<User?> GetUserByIdAsync(int id);

    Task<IEnumerable<User>> GetAllUsersAsync();
}