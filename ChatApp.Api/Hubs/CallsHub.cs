using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatApp.Api.Hubs;

[Authorize]
public class CallsHub : Hub
{
    public async Task CallUser(string userId)
    {
        var userIdClaim = Context!.User!.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine("Detta är id:t till den som ringer: " + userIdClaim);
        await Clients.User(userId).SendAsync("IncomingCall", userIdClaim!, Context?.User?.Identity?.Name);
    }

    public async Task AnswerCall(string callingUserId)
    {
        await Clients.User(callingUserId).SendAsync("AcceptCall", $"{Context?.User?.Identity?.Name} accepterade samtalet");
    }

    public async Task DeclineCall(string callingUserId)
    {
        await Clients.User(callingUserId).SendAsync("DeclineCall", $"{Context?.User?.Identity?.Name} declined the call.");
    }
}
