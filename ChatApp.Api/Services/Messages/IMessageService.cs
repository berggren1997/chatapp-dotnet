using ChatApp.Shared.DTO.Messages;
using ChatApp.Shared.Requests.Messages;

namespace ChatApp.Api.Services.Messages;

public interface IMessageService
{
    Task SendMessage(MessageRequest messageRequest, string senderName);

    // TODO: Meddelanden kan öka fort.. Se till att hämta ett gäng åt gången
    Task<IEnumerable<MessageDto>> GetMessages(Guid conversationId, string requesterUsername);
}
