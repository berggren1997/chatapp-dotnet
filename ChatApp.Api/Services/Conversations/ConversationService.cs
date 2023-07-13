using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Models.Exceptions.NotFoundExceptions;
using ChatApp.Shared.DTO.Conversations;
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
            .Where(uc => uc.Creator.UserName == username || 
                uc.Recipient.UserName == username)
            .ToListAsync();

        var formattedConversations = conversations.Select(c => new ConversationDto
        {
            Id = c.Id,
            CreatedAt = c.CreatedAt,
            ConversationDetails = new ConversationDetailDto
            {
                Creator = c.Creator?.UserName!,
                Recipient = c.Recipient?.UserName!
            }
        }).ToList();

        return formattedConversations;
    }
}
