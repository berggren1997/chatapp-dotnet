namespace ChatApp.Shared.Response.Messages;

public class MessageResponse
{
    public Guid ConversationId { get; set; }
    public string Sender { get; set; }
    public DateTime SentAt { get; set; }
    public string Message { get; set; }
}
