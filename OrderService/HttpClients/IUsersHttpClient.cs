using UserService.Models;

namespace OrderService.HttpClients;

public interface IUsersHttpClient
{
    Task<User> GetByIdAsync(Guid id);
}