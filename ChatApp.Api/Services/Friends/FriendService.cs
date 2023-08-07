using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Models.Exceptions.NotFoundExceptions;
using ChatApp.Shared.DTO.Friends;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Services.Friends;

public class FriendService : IFriendService
{
    private readonly ChatAppContext _chatAppContext;

    public FriendService(ChatAppContext chatAppContext)
    {
        _chatAppContext = chatAppContext;
    }

    public async Task<IEnumerable<FriendRequestDto>> GetFriendRequests(string appUserName)
    {
        AppUser appUser = await _chatAppContext.Users.FirstOrDefaultAsync(u => u.UserName!.Equals(appUserName)) ??
            throw new UserNotFoundException($"The user with name: {appUserName} was not found.");


        List<FriendRequest> friendRequests = await _chatAppContext.FriendRequests
            .Include(from => from.FromUser)
            .Where(fr => fr.ToUser.UserName == appUser.UserName && fr.RequestStatus.Equals(FriendRequestStatus.Pending))
            .ToListAsync();

        IEnumerable<FriendRequestDto> friendRequestsToReturn = friendRequests.Select(fr => new FriendRequestDto
        {
            FriendRequestSentAt = fr.FriendRequestDate,
            Id = fr.Id,
            FromUser = fr.FromUser!.UserName!
        });

        return friendRequestsToReturn;

    }

    public async Task<bool> SendFriendRequest(string appUserName, Guid toUserId)
    {
        var toUser = await _chatAppContext.Users.FirstOrDefaultAsync(x => x.Id.Equals(toUserId)) ??
            throw new UserNotFoundException($"The user with id: {toUserId} was not found.");

        var appUser = await _chatAppContext.Users.FirstOrDefaultAsync(ap => ap.UserName.Equals(appUserName));

        if (appUser == null) return false;

        if ((!await FriendRequestExists(appUser.Id, toUser.Id)) && (!await AlreadyFriends(appUser, toUser)))
        {
            var friendRequest = new FriendRequest
            {
                Id = Guid.NewGuid(),
                FriendRequestDate = DateTime.Now,
                FromUser = appUser,
                ToUser = toUser,
                RequestStatus = FriendRequestStatus.Pending
            };

            _chatAppContext.FriendRequests.Add(friendRequest);

            return await _chatAppContext.SaveChangesAsync() > 0;
        }

        return false;

    }

    private async Task<bool> FriendRequestExists(Guid fromUserId, Guid toUserId)
    {
        //var friendRequest = await _chatAppContext.FriendRequests
        //    .Where(fr => fr.FromUserId.Equals(fromUserId) && fr.ToUserId.Equals(toUserId))
        //    .FirstOrDefaultAsync();

        var friendRequest2 = await _chatAppContext.FriendRequests
            .AnyAsync(fr => fr.FromUserId.Equals(fromUserId) && fr.ToUserId.Equals(toUserId));

        return friendRequest2;
    }

    private async Task<bool> AlreadyFriends(AppUser fromUser, AppUser toUser)
    {
        var friendshipExits = await _chatAppContext.Friends
            .AnyAsync(f => f.AppUser.Id == fromUser.Id && f.FriendUser.Id == toUser.Id ||
                f.AppUser.Id == toUser.Id && f.FriendUser.Id == fromUser.Id);
        //var isAlreadyFriends = await _chatAppContext.Friends
        //.Any(f => (f.AppUser.Id == fromUser.Id && f.FriendUser.Id == toUser.Id) ||
        //          (f.AppUser.Id == toUser.Id && f.FriendUser.Id == fromUser.Id));

        return friendshipExits;
    }

    public async Task<IEnumerable<AppUser>> TempMethod()
    {
        return await _chatAppContext.Users.ToListAsync();
    }

    public async Task<bool> AcceptFriendRequest(Guid friendRequestId, string appUserName)
    {
        var friendRequest = await _chatAppContext.FriendRequests
            .FirstOrDefaultAsync(fr => fr.Id.Equals(friendRequestId));

        if (friendRequest == null) return false;

        friendRequest.RequestStatus = FriendRequestStatus.Accepted;

        var fromUser = await _chatAppContext.Users
            .FirstOrDefaultAsync(fr => fr.Id.Equals(friendRequest.FromUserId));

        var appUser = await _chatAppContext.Users
            .FirstOrDefaultAsync(ap => ap.Id.Equals(friendRequest.ToUserId));

        // Kollar så att användarna finns, OCH användaren som skickas reqeusten faktiskt är den som blivit förfrågad. Endast då
        // ska det gå att acceptera en friend request.
        if (fromUser != null && appUser != null && friendRequest.ToUser.UserName == appUserName)
        {
            return await CreateFriendship(fromUser, appUser);
        }

        return false;
    }

    private async Task<bool> CreateFriendship(AppUser fromUser, AppUser appUser)
    {
        var friend = new Friend
        {
            Id = Guid.NewGuid(),
            AppUser = appUser,
            FriendUser = fromUser,
            FriendshipDate = DateTime.Now
        };

        //friend.Friends.Add(fromUser);

        _chatAppContext.Friends.Add(friend);

        return await _chatAppContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<FriendDto>> GetFriends(string appUserName)
    {
        var friends = await _chatAppContext
            .Friends
            .Include(u => u.AppUser)
            .Include(f => f.FriendUser)
            .Where(u => u.AppUser.UserName!.Equals(appUserName) || u.FriendUser.UserName.Equals(appUserName))
            .ToListAsync();

        var friendDtos = friends
            .Select(f => new FriendDto
            {
                Id = f.FriendUser.Id,
                Name = f.FriendUser.UserName == appUserName ? f.AppUser.UserName : f.FriendUser.UserName
            }).ToList();

        return friendDtos;
    }
}
