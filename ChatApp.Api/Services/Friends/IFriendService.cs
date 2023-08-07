using ChatApp.Api.Models;
using ChatApp.Shared.DTO.Friends;

namespace ChatApp.Api.Services.Friends;

public interface IFriendService
{
    Task<bool> SendFriendRequest(string appUserName, Guid toUserId);
    Task<IEnumerable<FriendRequestDto>> GetFriendRequests(string appUserName);

    Task<bool> AcceptFriendRequest(Guid friendRequestId, string appUserName);
    Task<IEnumerable<FriendDto>> GetFriends(string appUserName);
    
    //TODO: En temporär metod för att enkelt kunna hämta ut alla användare. Ta bort när jag är klar.
    Task<IEnumerable<AppUser>> TempMethod();
}
