using ChatApp.Shared.DTO.Messages;
using ChatApp.Shared.Requests.Messages;
using ChatApp.Shared.Requests.RequestFeatures;
using ChatApp.Shared.Requests.RequestFeatures.Messages;

namespace ChatApp.Api.Services.Messages;

public interface IMessageService
{
    Task<bool> SendMessage(MessageRequest messageRequest, string senderName);

    // TODO: Meddelanden kan öka fort.. Se till att hämta ett gäng åt gången
    Task<(IEnumerable<MessageDto> messages, MetaData metaData)> GetMessages(Guid conversationId, string requesterUsername,
        MessageParams messageParams);
}
