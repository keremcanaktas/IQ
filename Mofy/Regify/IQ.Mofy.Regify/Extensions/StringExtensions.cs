namespace IQ.Mofy.Regify.Extensions;

internal static class StringExtensions
{
    internal static string? TrimStart(this string? source, string value, StringComparison stringComparison = StringComparison.Ordinal) => source?.StartsWith(value, stringComparison) ?? false ? source.Substring(value.Length) : source;
    internal static string? TrimEnd(this string? source, string value, StringComparison stringComparison = StringComparison.Ordinal) => source?.EndsWith(value, stringComparison) ?? false ? source.Remove(source.LastIndexOf(value, stringComparison)) : source;
}