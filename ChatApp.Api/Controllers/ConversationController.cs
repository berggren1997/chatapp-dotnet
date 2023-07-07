using ChatApp.Api.Services.Conversations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController : ControllerBase
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetConversations()
    {
        var username = GetCurrentUsername();
        var userConversations = await _conversationService.GetConversations(username!);
        return Ok(userConversations);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> CreateConversation(string recipient)
    {
        var creatorName = GetCurrentUsername();

        var newConversationId = await _conversationService.CreateConversation(creatorName!, recipient);

        return Ok(newConversationId);
    }

    private string? GetCurrentUsername() => User!.Identity?.Name;
}
