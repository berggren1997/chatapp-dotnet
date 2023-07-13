using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Models.Exceptions.NotFoundExceptions;
using ChatApp.Shared.DTO.Messages;
using ChatApp.Shared.Requests.Messages;
using ChatApp.Shared.Requests.RequestFeatures;
using ChatApp.Shared.Requests.RequestFeatures.Messages;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Messages;

public class MessageService : IMessageService
{
    private readonly ChatAppContext _appContext;

    public MessageService(ChatAppContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<(IEnumerable<MessageDto> messages, MetaData metaData)> GetMessages(Guid conversationId,
        string requesterUsername, MessageParams messageParams)
    {
        var user = await _appContext.Users
            .FirstOrDefaultAsync(u => u.UserName == requesterUsername) ??
            throw new UserNotFoundException(requesterUsername);

        var conversation = await GetConversation(conversationId) ??
            throw new ConversationNotFoundException(conversationId);

        if (IsEligibleForConversation(user, conversation))
        {
            var messages = await GetPaginatedMessages(conversationId,
                messageParams.PageNumber, messageParams.PageSize);

            var formattedMessages = FormattedMessages(messages);

            return (messages: formattedMessages, metaData: messages.MetaData);
        }

        throw new Exception(@"User not authorized. Should throw some custom exception, 
            and return 401.");
        
    }

    public async Task<bool> SendMessage(MessageRequest messageRequest, string senderName)
    {
        var conversation = await GetConversation(messageRequest.ConversationId) ??
            throw new ConversationNotFoundException(messageRequest.ConversationId);

        var user = await _appContext.Users
            .FirstOrDefaultAsync(u => u.UserName == senderName) ??
            throw new UserNotFoundException(senderName);

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

    private static IEnumerable<MessageDto> FormattedMessages(IEnumerable<Message> messages) =>
        messages.Select(m => new MessageDto
        {
            Message = m.Content!,
            Sender = m.Sender?.UserName!,
            SentAt = m.CreatedAt
        }).ToList();

    private async Task<Conversation?> GetConversation(Guid conversationId)
    {
        return await _appContext.Conversations
            .Include(c => c.Creator)
            .Include(r => r.Recipient)
            .FirstOrDefaultAsync(c => c.Id == conversationId);
    }

    private async Task<PagedList<Message>> GetPaginatedMessages(Guid conversationId, 
        int pageNumber, int pageSize)
    {
        var messages = await _appContext.Messages
                .Include(s => s.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderByDescending(d => d.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();

        var count = await _appContext.Messages
                .Where(m => m.ConversationId == conversationId)
                .CountAsync();

        return PagedList<Message>
            .ToPagedList(messages, count, pageNumber, pageSize);
    }
}
