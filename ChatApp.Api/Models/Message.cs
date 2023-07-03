namespace ChatApp.Api.Models;

public class Message
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public AppUser Sender { get; set; }
    public Guid ConversationId { get; set; }
}
