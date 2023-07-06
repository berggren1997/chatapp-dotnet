using ChatApp.Api.Services.Messages;
using ChatApp.Shared.Requests.Messages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> PostMessage([FromBody] MessageRequest request)
    {
        var username = User!.Identity!.Name;

        //TODO: Returnera bool från SendMessage
        await _messageService.SendMessage(request, username!);

        return Ok();
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetMessages(Guid conversationId)
    {
        var username = User!.Identity!.Name;
        var messages = await _messageService.GetMessages(conversationId, username!);

        return messages.Count() != 0 ? Ok(messages) : NotFound("No messages");
    }
}
