using ChatApp.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Api.Data;

public class ChatAppContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ChatAppContext(DbContextOptions<ChatAppContext> options) : base(options)
    { }

    public DbSet<Conversation> Conversations { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Friend> Friends { get; set; }
    public DbSet<FriendRequest> FriendRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Conversation>()
            .HasOne(x => x.Creator)
            .WithMany()
            .HasForeignKey(x => x.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Conversation>()
            .HasOne(r => r.Recipient)
            .WithMany()
            .HasForeignKey(r => r.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FriendRequest>()
            .HasOne(fr => fr.FromUser)
            .WithMany()
            .HasForeignKey(fr => fr.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FriendRequest>()
            .HasOne(fr => fr.ToUser)
            .WithMany()
            .HasForeignKey(fr => fr.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Friend>()
        .HasOne(f => f.AppUser)
        .WithMany()
        .HasForeignKey(f => f.AppUserId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Friend>()
            .HasOne(f => f.FriendUser)
            .WithMany()
            .HasForeignKey(f => f.FriendUserId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(builder);
    }
}
