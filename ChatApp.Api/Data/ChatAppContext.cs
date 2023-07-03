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
        //modelBuilder.Entity<Conversation>()
        //        .HasOne(x => x.Crea)
        //        .WithMany(x => x.Conversations)
        //        .OnDelete(DeleteBehavior.Restrict);
        base.OnModelCreating(builder);
    }
}
