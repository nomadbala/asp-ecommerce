using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserService.Contracts;

namespace UserService.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserRoles
{
    Client = 0,
    Administrator = 1
}
//
[Table("Users")]
public class User
{
    public User()
    {
    }

    public Guid Id { get; set; }

    public string FullName { get; set; }
    
    public string Email { get; set; }
    
    public string Address { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public UserRoles Role { get; set; }

    public User(CreateUserContract contract)
    {
        Id = Guid.NewGuid();
        FullName = contract.FullName;
        Email = contract.Email;
        Address = contract.Address;
        RegistrationDate = DateTime.UtcNow;
        Role = contract.Role;
    }
}