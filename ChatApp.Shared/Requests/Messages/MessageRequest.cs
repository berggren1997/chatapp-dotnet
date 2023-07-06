using System.ComponentModel.DataAnnotations;

namespace ChatApp.Shared.Requests.Messages;

public class MessageRequest : IValidatableObject
{
    [Required]
    public Guid ConversationId { get; set; }
    
    public string Message { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = new List<ValidationResult>();

        if (Message == null || Message.Trim().Length < 1)
        {
            validationResults.Add(new ValidationResult("Message is a required field",
                    new[] { "Message" }));
        }

        return validationResults;
    }
}
