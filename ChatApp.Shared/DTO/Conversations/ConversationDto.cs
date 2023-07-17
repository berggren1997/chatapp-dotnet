using ChatApp.Shared.DTO.Messages;

namespace ChatApp.Shared.DTO.Conversations;

public class ConversationDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public ConversationDetailDto ConversationDetails { get; set; } = new();
    public string LastMessage { get; set; } = string.Empty;
    public MessageDto? LastMessageDetails { get; set; }
}
