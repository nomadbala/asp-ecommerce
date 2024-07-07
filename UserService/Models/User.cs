using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models;

[Table("users")]
public class User
{
    public Guid Id { get; set; }
    
    
}