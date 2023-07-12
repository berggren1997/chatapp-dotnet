namespace ChatApp.Api.Models.Exceptions.NotFoundExceptions;

public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(string username) : base($"User: {username} was not found.")
    { }
}
