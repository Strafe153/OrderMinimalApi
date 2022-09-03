using FluentValidation.Results;

namespace OrderMinimalApi.Extensions
{
    public static class IDictionaryExtensions
    {
        public static IDictionary<string, string[]> ToDictionary(this List<ValidationFailure> validationFailures)
        {
            Dictionary<string, string[]> failuresDictionary = new();

            var groupedFailures = validationFailures
                .GroupBy(f => f.PropertyName)
                .Select(g => new
                {
                    Property = g.Key,
                    Errors = g.ToArray()
                });

            foreach (var failure in groupedFailures)
            {
                failuresDictionary.Add(failure.Property, failure.Errors.Select(f => f.ErrorMessage).ToArray());
            }

            return failuresDictionary;
        }
    }
}
