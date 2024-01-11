using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string Email { get; set; }
    
    [Required]
    public Role Role { get; set; }
}

public enum Role
{
    Admin,
    User
}