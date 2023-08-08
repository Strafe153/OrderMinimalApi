using FluentValidation.Results;

namespace Core.Extensions;

public static class IDictionaryExtensions
{
    public static IDictionary<string, string[]> ToDictionary(this List<ValidationFailure> validationFailures)
    {
        var failuresDictionary = new Dictionary<string, string[]>();

        var groupedFailures = validationFailures
            .GroupBy(f => f.PropertyName)
            .Select(g => new
            {
                PropertyName = g.Key,
                Failures = g.ToArray()
            });

        foreach (var failure in groupedFailures)
        {
            failuresDictionary.Add(failure.PropertyName, failure.Failures.Select(f => f.ErrorMessage).ToArray());
        }

        return failuresDictionary;
    }
}
