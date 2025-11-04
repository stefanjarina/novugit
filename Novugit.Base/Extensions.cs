using System.Globalization;

namespace Novugit.Base;

public static class Extensions
{
    public static string Capitalize(this String input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return string.Concat(input[..1].ToUpper(CultureInfo.CurrentCulture), input.AsSpan(1, input.Length - 1));
    }
}