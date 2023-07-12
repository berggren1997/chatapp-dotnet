namespace ChatApp.Api.Models.Exceptions.BadRequestExceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    { }
}
