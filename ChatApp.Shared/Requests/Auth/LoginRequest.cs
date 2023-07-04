using System.ComponentModel.DataAnnotations;

namespace ChatApp.Shared.Requests.Auth;

public class LoginRequest : IValidatableObject
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();

        if (Username == null || Username.Length < 5)
        {
            validationResults.Add(new
                ValidationResult("Username has to be at least 5 characters long",
                new[] { "Username" }));
        }

        if (Password == null || Password.Length < 5)
        {
            validationResults.Add(new
                ValidationResult("Password has to be at least 5 characters long",
                new[] { "Password" }));
        }
        return validationResults;
    }
}
