using Core.Exceptions;
using Core.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace MinimalApi;

public class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var statusCode = GetHttpStatusCode(exception);
        var statusCodeAsInt = (int)statusCode;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCodeAsInt;

        var problemDetails = new ProblemDetails
        {
            Type = GetRFCType(statusCode),
            Title = exception.Message,
            Status = statusCodeAsInt,
            Detail = exception.Message,
            Instance = httpContext.Request.Path
        };

        var json = JsonSerializer.Serialize(problemDetails);
        await httpContext.Response.WriteAsync(json);

        return true;
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
