using System.Net;
using Shop.UseCases.Exceptions;

namespace Shop.Web.Utils;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (EntityNotFoundException e)
        {
            await HandleException(httpContext, e.Message, HttpStatusCode.NotFound);
        }
    }

    private async Task HandleException(HttpContext httpContext, string message, HttpStatusCode code)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)code;

        await httpContext.Response.WriteAsync(message);
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}

