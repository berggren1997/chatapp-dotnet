using ChatApp.Api.Data;
using ChatApp.Api.Models.Exceptions.NotFoundExceptions;
using ChatApp.Shared.DTO.Users;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Users;

public class UserService : IUserService
{
    private readonly ChatAppContext _context;

    public UserService(ChatAppContext context)
    {
        _context = context;
    }

    public async Task<UserDto> FindUserByName(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        if( user == null)
        {
            throw new UserNotFoundException(username);
        }

        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName!
        };
    }

    public async Task<IEnumerable<UserDto>> FindUsers(string username)
    {
        var users = await _context.Users
            .Where(u => u.UserName!.Contains(username))
            .AsNoTracking()
            .ToListAsync();

        if (users.Any())
        {
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName!
            }).ToList();
        }

        return Enumerable.Empty<UserDto>();
    }
}
