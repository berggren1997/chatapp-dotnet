using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Services.Auth;
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

    //public static void ConfigureAuthentication(this IServiceCollection services)
    //{
    //    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //        .AddCookie("default", options =>
    //        {
    //            options.Cookie.Name = "qid";
    //            //options.ExpireTimeSpan = TimeSpan.FromDays(1);
    //            options.SlidingExpiration = true;
    //        });
    //}

    public static void ConfigureCookiePolicy(this WebApplication app)
    {
        var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
            HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
            Secure = CookieSecurePolicy.None,
        };

        app.UseCookiePolicy(cookiePolicyOptions);
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
    }
}
