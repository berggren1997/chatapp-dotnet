using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Models.Exceptions.BadRequestExceptions;
using ChatApp.Api.Models.Exceptions.NotFoundExceptions;
using ChatApp.Shared.DTO.Conversations;
using ChatApp.Shared.DTO.Messages;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Conversations;

public class ConversationService : IConversationService
{
    private readonly ChatAppContext _chatAppContext;

    public ConversationService(ChatAppContext chatAppContext)
    {
        _chatAppContext = chatAppContext;
    }

    // we will grab creatorName from claims if user is authenticated
    public async Task<Guid> CreateConversation(string creatorName, string recipient)
    {
        var creatorUser = await _chatAppContext.Users
            .FirstOrDefaultAsync(c => c.UserName == creatorName) ??
                throw new UserNotFoundException(creatorName);

        var recipientUser = await _chatAppContext.Users
            .FirstOrDefaultAsync(c => c.UserName == recipient) ??
            throw new UserNotFoundException(recipient);

        if (!await IsEligibleToCreateConversation(creatorName, recipient))
            throw new ConversationBadRequestException("Conversation between the two parties already exist.");

        var newConversation = new Conversation
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.Now,
            Creator = creatorUser,
            Recipient = recipientUser
        };

        _chatAppContext.Conversations.Add(newConversation);
        await _chatAppContext.SaveChangesAsync();

        return newConversation.Id;
    }

    // TODO: Hämta bara x-antal konversationer
    public async Task<IEnumerable<ConversationDto>> GetConversations(string username)
    {
        var conversations = await _chatAppContext.Conversations
            .Include(c => c.Creator)
            .Include(c => c.Recipient)
            .Include(m => m.Messages)
            .Where(uc => uc.Creator.UserName == username ||
                uc.Recipient.UserName == username)
            .Select(m => new
            {
                Conversation = m,
                LastMessage = m.Messages
                .OrderByDescending(d => d.CreatedAt)
                .FirstOrDefault()
            })
            .ToListAsync();


        var formattedConversations = conversations.Select(c => new ConversationDto
        {
            Id = c.Conversation.Id,
            CreatedAt = c.Conversation.CreatedAt,
            ConversationDetails = new ConversationDetailDto
            {
                Creator = c.Conversation.Creator?.UserName!,
                Recipient = c.Conversation.Recipient?.UserName!
            },
            LastMessageDetails = new MessageDto
            {
                Sender = c.LastMessage?.Sender.UserName!,
                Message = c.LastMessage?.Content!
            }
        }).ToList();

        return formattedConversations;
    }

    public async Task<ConversationDto> GetConversation(Guid conversationId)
    {
        var conversation = await _chatAppContext.Conversations
            .Include(c => c.Creator)
            .Include(c => c.Recipient)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        return new ConversationDto
        {
            Id = conversationId,
            CreatedAt = conversation.CreatedAt,
            ConversationDetails = new ConversationDetailDto
            {
                Creator = conversation.Creator?.UserName!,
                Recipient = conversation.Recipient?.UserName!
            }
        };
    }

    private async Task<bool> IsEligibleToCreateConversation(string creator, string recipient)
    {
        var conversation = await _chatAppContext.Conversations
            .Include(c => c.Creator)
            .Include(c => c.Recipient)
            .FirstOrDefaultAsync(c => c.Creator.UserName == creator && c.Recipient.UserName == recipient
            || c.Creator.UserName == recipient && c.Recipient.UserName == creator);

        return conversation == null;
    }
}
