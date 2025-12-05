using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace MinimalApi;

public class ExceptionHandler : IExceptionHandler
{
	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		var statusCode = GetHttpStatusCode(exception);
		
		await Results
			.Problem(exception.Message, httpContext.Request.Path, (int)statusCode)
			.ExecuteAsync(httpContext);

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
}
