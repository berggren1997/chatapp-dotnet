namespace ChatApp.Api.Models;

public class Friend
{
    public Guid Id { get; set; }
    public DateTime FriendshipDate { get; set; }
    public AppUser? AppUser { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? FriendUser { get; set; }
    public Guid FriendUserId { get; set; }
    //public List<AppUser> Friends { get; set; } = new();
}
