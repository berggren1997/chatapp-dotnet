using ChatApp.Shared.Requests.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs;

[Authorize]
public class MessageHub : Hub
{
    private async Task<bool> SendMessageAsync(MessageRequest messageRequest)
    {
        await Console.Out.WriteLineAsync();
        return true;
    }
}
