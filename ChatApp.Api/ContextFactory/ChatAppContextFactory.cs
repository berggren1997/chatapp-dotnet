using ChatApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ChatApp.Api.ContextFactory;

public class ChatAppContextFactory : IDesignTimeDbContextFactory<ChatAppContext>
{
    public ChatAppContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<ChatAppContextFactory>()
                .Build();

        var connectionstring = configuration.GetConnectionString("MSSQLConnection");
        var serverVersion = new MySqlServerVersion(new Version(8, 5, 2));

        var builder = new DbContextOptionsBuilder<ChatAppContext>()
            .UseSqlServer(connectionstring,
            b => b.MigrationsAssembly("ChatApp.Api"));

        return new ChatAppContext(builder.Options);
    }
}
