namespace ChatApp.Shared.Response.Auth;

public class LoginResponse
{
    public bool Success { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
