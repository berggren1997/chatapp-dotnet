namespace ChatApp.Api.Models.Exceptions.BadRequestExceptions;

public class MessageBadRequestException : BadRequestException
{
    public MessageBadRequestException(string message) : base(message)
    { }
}
