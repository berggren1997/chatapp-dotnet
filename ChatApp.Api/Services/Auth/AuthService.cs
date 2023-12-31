﻿using ChatApp.Api.Models;
using ChatApp.Shared.Requests.Auth;
using ChatApp.Shared.Response.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task RevokeSession(Guid userId)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);

        if (user != null)
        {
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
        }
    }

    public async Task<bool> Login(LoginRequest loginRequest)
    {
        AppUser? user = await _userManager.FindByNameAsync(loginRequest.Username);

        if (user != null)
        {
            bool successFulSignIn = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            
            if (successFulSignIn)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                return true;
            }
        }

        return false;
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
