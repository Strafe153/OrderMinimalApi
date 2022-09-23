using OrderMinimalApi.Models;
using System.Net;

namespace OrderMinimalApi.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NullReferenceException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound,
                $"{ex.Message}. Path:{context.Request.Path}.");
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError,
               $"{ex.Message}. Path:{context.Request.Path}.");
        }
    }

    private Task HandleExceptionAsync(HttpContext context, HttpStatusCode code, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(
            new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
    }
}
