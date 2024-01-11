using System.ComponentModel.DataAnnotations;

namespace Service.CommandQuerryModels.CommandModels;

public class CreateUserCommand
{
    [Required(ErrorMessage = "Full name is required."), MinLength(3, ErrorMessage = "Full name must be at least 3 characters long."), MaxLength(20, ErrorMessage = "Full name cannot exceed 20 characters.")]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Email is required."), EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required."), MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string Password { get; set; }
}