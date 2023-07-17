using ChatApp.Api.Hubs;
using ChatApp.Api.Services.Messages;
using ChatApp.Shared.Requests.Messages;
using ChatApp.Shared.Requests.RequestFeatures.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly IHubContext<MessageHub> _hubContext;

    public MessageController(IMessageService messageService, IHubContext<MessageHub> hubContext)
    {
        _messageService = messageService;
        _hubContext = hubContext;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> PostMessage([FromBody] MessageRequest request) 
    {
        string? username = User!.Identity!.Name;

        //TODO: Kasta ett custom-fel så att rätt statuskod returneras
        // ifall insättning av meddelande misslyckas
        bool success = await _messageService.SendMessage(request, username!);

        return success ? Ok() : BadRequest();
        
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetMessages(Guid conversationId,
        [FromQuery] MessageParams messageParams)
    {
        var username = User!.Identity!.Name;
        var (messages, metaData) = await _messageService.GetMessages(conversationId, username!, messageParams);

        return messages.Any() ? Ok(new { messages, metaData }) : NotFound();
    }
}
