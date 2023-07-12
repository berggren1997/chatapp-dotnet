using ChatApp.Api.Models.ErrorModel;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ChatApp.Api.Extensions;

public static class ExceptionMiddleware
{
    public static void ConfigureExceptionMiddlewareHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            app.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature is not null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var errorDetail = new ErrorDetail
                    {
                        StatusCode = context.Response.StatusCode,
                        ErrorMessage = contextFeature.Error.Message,
                    }.ToString();

                    await context.Response.WriteAsync(errorDetail!);
                }
            });
        });
    }
}
