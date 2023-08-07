namespace ChatApp.Api.Models;

public class FriendRequest
{
    public Guid Id { get; set; }
    public DateTime FriendRequestDate { get; set; }
    public string? RequestStatus { get; set; }
    public AppUser FromUser { get; set; }
    public Guid FromUserId { get; set; }
    public AppUser ToUser { get; set; }
    public Guid ToUserId { get; set; }
}
