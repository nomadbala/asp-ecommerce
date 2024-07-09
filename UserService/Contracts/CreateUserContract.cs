using UserService.Models;

namespace UserService.Contracts;

public record CreateUserContract(string FullName, string Email, string Address, UserRoles Role);