namespace ChatApp.Shared.Response.Messages;

public class MessageResponse
{
    public string Sender { get; set; }
    public DateTime SentAt { get; set; }
    public string Message { get; set; }
}
