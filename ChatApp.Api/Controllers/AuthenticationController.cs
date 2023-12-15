using ChatApp.Api.Services.Auth;
using ChatApp.Shared.Requests.Auth;
using ChatApp.Shared.Response.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private static int _requests;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
        _requests = 1;
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var registerRequest = await _authService.Register(request);

        if (!registerRequest.Success)
        {
            return BadRequest(registerRequest);
        }

        return Ok(registerRequest);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        var loginSuccess = await _authService.Login(request);

        if (!loginSuccess)
        {
            return BadRequest(new LoginResponse
            {
                Success = false,
                ErrorMessage = "Bad credentials."
            });
        }

        return Ok(new LoginResponse
        {
            Success = true,
            Username = request.Username
        });
    }

    [HttpGet("signout"), Authorize]
    public async Task<IActionResult> LogOut()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        await _authService.RevokeSession(Guid.Parse(userId));
        return Ok();
    }

    [HttpGet("me"), Authorize]
    public ActionResult ProtectedRoute()
    {
        Console.WriteLine("Request: " + _requests++);
        return Ok(new
        {
            username = User!.Identity!.Name,
            userId = User.FindFirst(ClaimTypes.NameIdentifier).Value,
        });
    }

    //[Authorize(Roles = "Admin")]
    //[HttpGet("revoke")]
    //public async Task<IActionResult> RevokeSession(Guid userId)
    //{
    //    await _authService.RevokeSession(userId);

    //    return Ok();
    //}
}
