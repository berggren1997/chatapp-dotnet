using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Auth;
using ChatApp.Api.Services.Conversations;
using ChatApp.Api.Services.Messages;
using ChatApp.Api.Services.Users;
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

    public static void ConfigureMSSQLConnection(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ChatAppContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection")));
    }

    public static void ConfigureIdentity(this IServiceCollection services)
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

    public static void ConfigureCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddPolicy("DefaultPolicy", config =>
            {
                config.AllowAnyHeader();
                config.AllowAnyMethod();
                //config.AllowAnyOrigin();
                config.AllowCredentials();
                config.WithOrigins("http://localhost:3000", "http://localhost:5247");
            });
        });
    }

    public static void ConfigureCookieOptions(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(opt =>
        {
            opt.ExpireTimeSpan = TimeSpan.FromDays(100);
            opt.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            opt.Cookie.SameSite = SameSiteMode.None;
            opt.Cookie.HttpOnly = true;
            opt.SlidingExpiration = true;
            opt.Cookie.Name = "qid";
        });
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IUserService, UserService>();
    }
}
