namespace ChatApp.Api.Models.Exceptions.NotFoundExceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    { }
}
