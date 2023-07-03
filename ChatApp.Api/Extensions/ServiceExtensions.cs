using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace ChatApp.Api.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureMySqlConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionstring = configuration.GetConnectionString("MySQLConnection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

        services.AddDbContext<ChatAppContext>(options =>
            options.UseMySql(connectionstring, serverVersion));
    }

    public static void ConfigureEfCoreIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<AppUser, AppRole>(config =>
        {
            config.Password.RequireDigit = false;
            config.Password.RequireLowercase = false;
            config.Password.RequireUppercase = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequiredLength = 5;
            config.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<ChatAppContext>()
        .AddDefaultTokenProviders();
    }

    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}
