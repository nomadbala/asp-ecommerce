using UserService.Contracts;
using UserService.Models;

namespace UserService.Services;

public interface IUsersService
{
    Task<IEnumerable<User>> GetAllAsync();

    Task<User> CreateAsync(CreateUserContract contract);

    Task<User> GetByIdAsync(Guid id);

    Task<User> UpdateAsync(Guid id, UpdateUserContract contract);

    Task DeleteByIdAsync(Guid id);

    Task<IEnumerable<User>> GetByNameAsync(string fullName);

    Task<User> GetByEmailAsync(string email);
}