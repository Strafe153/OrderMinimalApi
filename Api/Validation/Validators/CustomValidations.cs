using System.Text.RegularExpressions;
using Api.Constants;

public static partial class CustomValidations
{
    public static bool BeInFullNameFormat(string name) =>
        name is not null && FullNameRegex().Match(name).Length > 0;

    [GeneratedRegex(ValidatorConstants.FullNamePattern)]
    private static partial Regex FullNameRegex();
}
