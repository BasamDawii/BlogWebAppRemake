using System.ComponentModel.DataAnnotations;

namespace Service.CommandQuerryModels.CommandModels;

public class UserLoginCommand
{
    [EmailAddress(ErrorMessage = "Invalid email address format."), Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}