using ChatApp.Api.Services;
using ChatApp.Shared.Requests.Auth;
using ChatApp.Shared.Response.Auth;
using Microsoft.AspNetCore.Mvc;

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

    [HttpPost]
    public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
    {
        var registerRequest = await _authService.Register(request);

        return registerRequest.Success ? Ok(registerRequest) : BadRequest(registerRequest);
    }
}
