using Core.Exceptions;
using Core.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MinimalApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
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

        var problemDetails = new ProblemDetails
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
            OperationFailedException
                or OperationCanceledException => HttpStatusCode.BadRequest,
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
