using ChatApp.Api.Models;
using ChatApp.Shared.Requests.Auth;
using ChatApp.Shared.Response.Auth;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;

    public AuthService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Login(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByNameAsync(loginRequest.Username);

        return user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password);
    }

    public async Task<RegisterResponse> Register(RegisterRequest registerRequest)
    {
        var newUser = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = registerRequest.Username,
            Email = registerRequest.Email
        };

        var result = await _userManager.CreateAsync(newUser, registerRequest.Password);

        return new RegisterResponse
        {
            Success = result.Succeeded,
            Errors = result?.Errors.Count() > 0
            ? result.Errors.Select(x => x.Description).ToList()
            : null
        };
    }
}
