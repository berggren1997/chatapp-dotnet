namespace ChatApp.Api.Models;

public class Conversation
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public AppUser Creator { get; set; }
    public Guid CreatorId { get; set; }
    public AppUser Recipient { get; set; }
    public Guid RecipientId { get; set; }
}
