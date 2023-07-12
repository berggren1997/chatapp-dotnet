using ChatApp.Api.Models.ErrorModel;
using ChatApp.Api.Models.Exceptions.BadRequestExceptions;
using ChatApp.Api.Models.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace ChatApp.Api.Extensions;

public static class ExceptionMiddleware
{
    public static void ConfigureExceptionMiddlewareHandler(this WebApplication app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        NotFoundException => StatusCodes.Status404NotFound,
                        BadRequestException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    await context.Response.WriteAsync(new ErrorDetail
                    {
                        StatusCode = context.Response.StatusCode,
                        ErrorMessage = contextFeature.Error.Message,
                    }.ToString());
                }
            });
        });
    }
}
