using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Shared;
using System.Net;
using System.Text.Json;

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
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetHttpStatusCode(exception);
        var statusCodeAsInt = (int)statusCode;

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCodeAsInt;

        var problemDetails = new ProblemDetails()
        {
            Type = GetRFCType(statusCode),
            Title = exception.Message,
            Status = statusCodeAsInt,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        var json = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(json);
    }

    private static HttpStatusCode GetHttpStatusCode(Exception exception) =>
        exception switch
        {
            NullReferenceException => HttpStatusCode.NotFound,
            OperationCanceledException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError
        };

    private static string GetRFCType(HttpStatusCode statusCode) =>
        statusCode switch
        {
            HttpStatusCode.NotFound => RFCType.NotFound,
            HttpStatusCode.BadRequest => RFCType.BadRequest,
            _ => RFCType.InternalServerError
        };
}
