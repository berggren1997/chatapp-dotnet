using ChatApp.Api.Models;
using ChatApp.Shared.DTO.Conversations;

namespace ChatApp.Api.Services.Conversations;

public interface IConversationService
{
    Task<Guid> CreateConversation(string creatorName, string recipient);
    Task<IEnumerable<ConversationDto>> GetConversations(string username);
}
