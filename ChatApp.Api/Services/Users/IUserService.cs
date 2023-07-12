using ChatApp.Shared.DTO.Users;

namespace ChatApp.Api.Services.Users;

public interface IUserService
{
    Task<IEnumerable<UserDto>> FindUsers(string username);
    Task<UserDto> FindUserByName(string username);
}
