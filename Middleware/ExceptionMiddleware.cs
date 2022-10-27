using Microsoft.AspNetCore.Mvc;
using OrderMinimalApi.Models;
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
        catch (NullReferenceException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message, RFCType.NotFound);
        }
        catch (OperationCanceledException ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message, RFCType.BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, ex.Message, RFCType.InternalServerError);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message, string rfcType)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var problemDetails = new ProblemDetails()
        {
            Type = rfcType,
            Title = message,
            Status = (int)statusCode,
            Detail = message,
            Instance = context.Request.Path
        };

        var json = JsonSerializer.Serialize(problemDetails);

        return context.Response.WriteAsync(json);
    }
}
