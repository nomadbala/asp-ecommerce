using UserService.Models;

namespace PaymentService.HttpClients;

public interface IUsersHttpClient
{
    Task<User> GetByIdAsync(Guid id);
}