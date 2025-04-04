using System.Globalization;
using System.Text;

namespace BitCrafts.Infrastructure.Abstraction.Extensions;

public static class StringExtension
{
    public static int ParseIntOrGetDefault(
        this string input,
        int defaultValue = 0,
        NumberStyles numberStyle = NumberStyles.Integer,
        IFormatProvider culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return int.TryParse(input, numberStyle, culture, out var result) ? result : defaultValue;
    }

    public static double ParseDoubleOrGetDefault(
        this string input,
        double defaultValue = 0.0,
        NumberStyles numberStyle = NumberStyles.Float | NumberStyles.AllowThousands,
        IFormatProvider culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return double.TryParse(input, numberStyle, culture, out var result) ? result : defaultValue;
    }

    public static decimal ParseDecimalOrGetDefault(
        this string input,
        decimal defaultValue = 0.0m,
        NumberStyles numberStyle = NumberStyles.Number,
        IFormatProvider culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return decimal.TryParse(input, numberStyle, culture, out var result) ? result : defaultValue;
    }

    public static bool ToBoolOrDefault(this string input, bool defaultValue = false)
    {
        return bool.TryParse(input, out var result) ? result : defaultValue;
    }

    public static string TrimOrEmpty(this string input)
    {
        return input?.Trim() ?? string.Empty;
    }

    public static string ExtractDigits(this string input)
    {
        var result = new StringBuilder();
        foreach (var ch in input)
            if (char.IsDigit(ch))
                result.Append(ch);

        return result.ToString();
    }

    public static DateTime ToDateTimeOrDefault(this string input, DateTime defaultValue, string format = "yyyy-MM-dd",
        IFormatProvider culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;
        return DateTime.TryParseExact(input, format, culture, DateTimeStyles.None, out var result)
            ? result
            : defaultValue;
    }

    public static bool IsNullOrWhiteSpace(this string input)
    {
        return string.IsNullOrWhiteSpace(input);
    }
}