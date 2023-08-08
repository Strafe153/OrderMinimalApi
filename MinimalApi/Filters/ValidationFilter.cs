using Core.Extensions;
using FluentValidation;

namespace MinimalApi.Filters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validatable = context.Arguments.SingleOrDefault(a => a?.GetType() == typeof(T)) as T;

        if (validatable is null)
        {
            return Results.BadRequest();
        }

        var validationResult = await _validator.ValidateAsync(validatable);

        if (!validationResult.IsValid)
        {
            var failuresDictionary = validationResult.Errors.ToDictionary();
            return Results.ValidationProblem(failuresDictionary);
        }

        return await next(context);
    }
}
