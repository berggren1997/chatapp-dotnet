using System.ComponentModel.DataAnnotations;

namespace ChatApp.Shared.Requests.Auth;

public class RegisterRequest : IValidatableObject
{
    [Required(ErrorMessage = "Email must be provided."), MinLength(15)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Username must be provided."), MinLength(5)]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password must be provided"), MinLength(5)]
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationErros = new List<ValidationResult>();

        if(Password == null || Password.Length < 5)
        {
            validationErros.Add(
                new ValidationResult("Password has to be at least 5 characters long", 
                    new[] { "Password" }));
        }

        if(ConfirmPassword == null || ConfirmPassword.Length < 5)
        {
            validationErros.Add(
                new ValidationResult("Password has to be at least 5 characters long",
                    new[] { "ConfirmPassword" }));
        }
        else if(ConfirmPassword != Password)
        {
            validationErros.Add(
                new ValidationResult("Confirm password has to match the password field.",
                    new[] { "ConfirmPassword" }));
        }

        return validationErros;
    }
}
