using System.ComponentModel.DataAnnotations.Schema;
using UserService.Contracts;

namespace UserService.Models;

public enum UserRoles
{
    Client = 0,
    Administrator = 1
}

[Table("users")]
public class User
{
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