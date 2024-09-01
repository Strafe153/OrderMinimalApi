using Domain.Exceptions;
using Domain.Shared;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;

namespace MinimalApi;

public class ExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var statusCode = GetHttpStatusCode(exception);
		var statusCodeAsInt = (int)statusCode;

		httpContext.Response.ContentType = MediaTypeNames.Application.Json;
		httpContext.Response.StatusCode = statusCodeAsInt;

		ProblemDetails problemDetails = new()
		{
			Type = GetRFCType(statusCode),
			Status = statusCodeAsInt,
			Detail = exception.Message,
			Instance = httpContext.Request.Path
		};

		var json = JsonConvert.SerializeObject(problemDetails);
		await httpContext.Response.WriteAsync(json, cancellationToken);

		return true;
	}

	private static HttpStatusCode GetHttpStatusCode(Exception exception) =>
		exception switch
		{
			NullReferenceException => HttpStatusCode.NotFound,
			OperationFailedException
				or OpenIddictApplicationNotFoundException
				or GrantNotImplementedException
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
