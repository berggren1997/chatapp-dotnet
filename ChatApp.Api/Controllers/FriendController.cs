using ChatApp.Api.Services.Friends;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FriendController : ControllerBase
{
    private readonly IFriendService _friendService;

    public FriendController(IFriendService friendService)
    {
        _friendService = friendService;
    }

    [HttpGet("friends"), Authorize]
    public async Task<IActionResult> GetFriends()
    {
        var appUserName = User!.Identity!.Name!;

        var friends = await _friendService.GetFriends(appUserName);
        
        return Ok(friends);
    }

    [HttpGet("friendrequests"), Authorize]
    public async Task<IActionResult> GetFriendRequests()
    {
        string appUserName = HttpContext.User.Identity!.Name!;

        var friendRequests = await _friendService.GetFriendRequests(appUserName);

        return Ok(friendRequests);
    }

    [HttpPost("accept-friendrequest"), Authorize]
    public async Task<IActionResult> AcceptFriendRequest(Guid friendRequestId)
    {
        string appUserName = HttpContext.User!.Identity!.Name!;
        bool result = await _friendService.AcceptFriendRequest(friendRequestId, appUserName);

        return result ? Ok(result) : BadRequest();
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersTemp()
    {
        return Ok(await _friendService.TempMethod());
    }

    [HttpPost("add"), Authorize]
    public async Task<IActionResult> SendFriendRequest(Guid toUserId)
    {
        string appUserName = HttpContext.User!.Identity!.Name!;

        bool result = await _friendService.SendFriendRequest(appUserName, toUserId);

        return result ? Ok(result) : BadRequest(result);
    }
}
