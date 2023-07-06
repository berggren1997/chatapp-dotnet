namespace ChatApp.Shared.DTO.Conversations;

public class ConversationDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public ConversationDetailDto ConversationDetails { get; set; } = new();
}
