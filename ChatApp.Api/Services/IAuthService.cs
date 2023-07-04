﻿using ChatApp.Shared.Requests.Auth;
using ChatApp.Shared.Response.Auth;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Api.Services;

public interface IAuthService
{
    Task<RegisterResponse> Register(RegisterRequest registerRequest);
    Task<bool> Login(LoginRequest loginRequest);
}
