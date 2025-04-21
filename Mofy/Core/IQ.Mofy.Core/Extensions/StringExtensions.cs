// ReSharper disable CheckNamespace
namespace System;

public static class StringExtensions
{
    public static string? TrimStart(this string? source, string value, StringComparison stringComparison = StringComparison.Ordinal) => source?.StartsWith(value, stringComparison) ?? false ? source[value.Length..] : source;

    public static string? TrimEnd(this string? source, string value, StringComparison stringComparison = StringComparison.Ordinal) => source?.EndsWith(value, stringComparison) ?? false ? source.Remove(source.LastIndexOf(value, stringComparison)) : source;
}