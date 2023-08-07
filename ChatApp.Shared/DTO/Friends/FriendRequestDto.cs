namespace ChatApp.Shared.DTO.Friends;

public class FriendRequestDto
{
    public Guid Id { get; set; }
    public string FromUser { get; set; } = string.Empty;
    public DateTime FriendRequestSentAt { get; set; }
}
