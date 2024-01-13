using System.ComponentModel.DataAnnotations;

namespace BlogWebAppTests.TestModels;

public class RegisterTestModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
