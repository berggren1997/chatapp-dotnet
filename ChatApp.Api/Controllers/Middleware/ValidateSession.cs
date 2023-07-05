using System.Net;
using System.Text;

namespace ChatApp.Api.Controllers.Middleware;

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
            if(!context.User.Identity.IsAuthenticated)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }

        await _next.Invoke(context);
    }
}
