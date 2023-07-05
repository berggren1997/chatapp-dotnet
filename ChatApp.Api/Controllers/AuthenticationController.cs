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

        if (!registerRequest.Success)
        {
            return BadRequest(registerRequest);
        }

        return Ok(registerRequest);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var loginSuccess = await _authService.Login(request);

        if (!loginSuccess)
        {
            return BadRequest("Bad credentials");
        }

        return Ok($"You are logged in as {request.Username}");
    }

    [HttpGet("test"), Authorize]
    public ActionResult ProtectedRoute()
    {
        return Ok("Protected route");
    }

    //[Authorize("Admin")]
    [HttpGet("revoke")]
    public async Task<IActionResult> RevokeSession(Guid userId)
    {
        await _authService.RevokeSession(userId);

        return Ok("All old usersessions should be inactive. Please log in again");
    }
}
