namespace ChatApp.Shared.DTO.Messages;

public class MessageDto
{
    public string Message { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
}
