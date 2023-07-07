using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Shared.DTO.Messages;
using ChatApp.Shared.Requests.Messages;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Messages;

public class MessageService : IMessageService
{
    private readonly ChatAppContext _appContext;

    public MessageService(ChatAppContext appContext)
    {
        _appContext = appContext;
    }

    
    //TODO: Lägg så att man hämtar x-antal meddelande åt gången
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

        if (conversation == null) {  return Enumerable.Empty<MessageDto>(); }

        // 3. Kolla att användaren är en del utav konversationen

        if(conversation.Recipient.Id == user.Id || conversation.Creator.Id == user.Id)
        {
            // 4. Hämta meddelanden
            var messages = await _appContext.Messages
                .Include(s => s.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();

            var messagesToReturn = messages.Select(m => new MessageDto
            {
                Message = m.Content,
                Sender = m.Sender.UserName,
                SentAt = m.CreatedAt
            }).ToList();

            return messagesToReturn;

        }
        else
        {
            return Enumerable.Empty<MessageDto>();
        }
    }

    public async Task SendMessage(MessageRequest messageRequest, string senderName)
    {
        var conversation = await _appContext.Conversations
            .Include(u => u.Creator) 
            .Include(r => r.Recipient)
            .FirstOrDefaultAsync(c => c.Id == messageRequest.ConversationId);

        if (conversation == null)
        {
            throw new Exception(@$"Conversation with id: 
                {messageRequest.ConversationId} doesnt exist");
        }

        var sender = await _appContext.Users
            .FirstOrDefaultAsync(u => u.UserName == senderName);

        if (sender == null)
        {
            throw new Exception("Error loading messages, user was not found");
        }
        else if((conversation.Creator.UserName != sender.UserName) &&
            (conversation.Recipient.UserName != sender.UserName))
        {
            throw new Exception($"User: {sender.UserName} is not part of that conversation.");
        }

        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = messageRequest.Message,
            ConversationId = conversation.Id,
            Sender = sender,
            CreatedAt = DateTime.Now
        };

        _appContext.Messages.Add(message);
        await _appContext.SaveChangesAsync();
    }

}
