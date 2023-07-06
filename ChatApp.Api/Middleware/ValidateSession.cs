using System.Net;
using System.Text;

namespace ChatApp.Api.Middleware;

public class ValidateSession
{
    private readonly RequestDelegate _next;
    private readonly List<string> _anonymousEndpoints = new() { "login", "register" };

    public ValidateSession(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {

        var endpointPath = context.Request.Path.ToString().ToLower().Split("/")[^1];

        if (!_anonymousEndpoints.Contains(endpointPath))
        {
            await Console.Out.WriteLineAsync(context.User.Identity.Name);
        }

        await _next.Invoke(context);
    }
}
