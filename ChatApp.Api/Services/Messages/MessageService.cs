using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Shared.DTO.Messages;
using ChatApp.Shared.Requests.Messages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ChatApp.Api.Services.Messages;

public class MessageService : IMessageService
{
    private readonly ChatAppContext _appContext;

    public MessageService(ChatAppContext appContext)
    {
        _appContext = appContext;
    }

    //TODO:
    //1. Lägg så att man hämtar x-antal meddelande åt gången
    //2. Kanske fixa ett middleware, som gör dessa kontroller på inkommande request,
    // för att korta ner metoden?
    public async Task<IEnumerable<MessageDto>> GetMessages(Guid conversationId,
        string requesterUsername)
    {
        // 1. Kolla att användaren finns
        var user = await _appContext.Users
            .FirstOrDefaultAsync(u => u.UserName == requesterUsername);

        if (user == null) { return Enumerable.Empty<MessageDto>(); }

        // 2. Kolla att konversationen finns
        var conversation = await _appContext.Conversations
            .Include(c => c.Creator)
            .Include(r => r.Recipient)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null) { return Enumerable.Empty<MessageDto>(); }

        // 3. Kolla att användaren är en del utav konversationen

        if (IsEligibleForConversation(user, conversation))
        {
            // 4. Hämta meddelanden
            var messages = await _appContext.Messages
                .Include(s => s.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();

            return FormattedMessages(messages);
        }

        return Enumerable.Empty<MessageDto>();
    }

    public async Task<bool> SendMessage(MessageRequest messageRequest, string senderName)
    {
        var conversation = await _appContext.Conversations
            .Include(u => u.Creator)
            .Include(r => r.Recipient)
            .FirstOrDefaultAsync(c => c.Id == messageRequest.ConversationId) ??
            throw new Exception(@$"Conversation with id: {messageRequest.ConversationId} doesnt exist");


        var user = await _appContext.Users
            .FirstOrDefaultAsync(u => u.UserName == senderName) ??
            throw new Exception("User does not exist.");

        if (IsEligibleForConversation(user, conversation))
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Content = messageRequest.Message,
                ConversationId = conversation.Id,
                Sender = user,
                CreatedAt = DateTime.Now
            };

            _appContext.Messages.Add(message);
            return await _appContext.SaveChangesAsync() > 0;
        }
        return false;
    }

    private static bool IsEligibleForConversation(AppUser user,
        Conversation conversation) =>
        conversation.Creator.UserName == user.UserName ||
            conversation.Recipient.UserName == user.UserName;

    private static IEnumerable<MessageDto> FormattedMessages(List<Message> messages) =>
        messages.Select(m => new MessageDto
        {
            Message = m.Content!,
            Sender = m.Sender?.UserName!,
            SentAt = m.CreatedAt
        }).ToList();
}
