namespace ChatApp.Api.Models.Exceptions.BadRequestExceptions;

public class ConversationBadRequestException : BadRequestException
{
    public ConversationBadRequestException(string message) : base(message)
    { }
}
