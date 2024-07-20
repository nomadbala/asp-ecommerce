using System.Text.Json.Serialization;
using UserService.Models;

namespace UserService.Contracts;

public record CreateUserContract(
    string FullName,
    string Email,
    string Address,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    UserRoles Role);