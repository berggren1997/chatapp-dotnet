namespace ChatApp.Shared.DTO.Conversations;

public class ConversationDetailDto
{
    public string Creator { get; set; } = string.Empty;
    public Guid? CreatorId { get; set; }
    public string Recipient { get; set; } = string.Empty;
    public Guid? RecipientId { get; set; }
}
