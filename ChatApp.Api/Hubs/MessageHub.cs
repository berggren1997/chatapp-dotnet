using ChatApp.Api.Services.Messages;
using ChatApp.Shared.Requests.Messages;
using ChatApp.Shared.Response.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Hubs;

[Authorize]
public class MessageHub : Hub
{
    private readonly IMessageService _messageService;

    public MessageHub(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task SendMessageAsync(MessageRequest messageRequest)
    {
        if (string.IsNullOrEmpty(messageRequest.Message.Trim())) return;

        string username = Context.User?.Identity!.Name!;

        bool savedMessage = await _messageService.SendMessage(messageRequest, username!);

        if (savedMessage)
        {
            var messageResponse = new MessageResponse
            {
                Sender = username,
                Message = messageRequest.Message,
                SentAt = DateTime.Now,
                ConversationId = messageRequest.ConversationId
            };

            await Clients.All.SendAsync("OnMessageReceived", messageResponse);
        }
    }
}
