using ChatApp.Api.Services;
using ChatApp.Shared.Requests.Auth;
using ChatApp.Shared.Response.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var registerRequest = await _authService.Register(request);

        if(!registerRequest.Success)
        {
            return BadRequest(registerRequest);
        }

        return Ok(registerRequest);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var loginSuccess = await _authService.Login(request);

        if(!loginSuccess)
        {
            return BadRequest("Bad credentials");
        }

        await CreateCookie(request.Username);

        return Ok($"You are logged in as {request.Username}");
    }

    [Authorize]
    [HttpGet("test")]
    public ActionResult ProtectedRoute()
    {
        var username = User?.Identity?.Name;
        return Ok($"Hello, {username}");
    }

    private async Task CreateCookie(string username)
    {
        var claims = new List<Claim>()
        {
            new Claim(type: ClaimTypes.Name, username)
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), 
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
           
            });


    }
}
