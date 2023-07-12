using ChatApp.Api.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("multiple/{username}")]
    public async Task<IActionResult> FindUsers(string username)
    {
        var users = await _userService.FindUsers(username);
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> FindUser(string username)
    {
        var user = await _userService.FindUserByName(username);
        return Ok(user);
    }
}
