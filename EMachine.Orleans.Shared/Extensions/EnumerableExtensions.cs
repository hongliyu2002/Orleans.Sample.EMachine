using System.Collections.Immutable;
using Orleans.FluentResults;

namespace EMachine.Orleans.Shared.Extensions;

/// <summary>
/// </summary>
public static class EnumerableExtensions
{
    public static string ToReason(this IEnumerable<IError> errors)
    {
        return string.Join(';', errors.OfType<Error>().Select(error => error.ToString()));
    }

    public static IImmutableList<string> ToReasons(this IEnumerable<IError> errors)
    {
        return errors.OfType<Error>().Select(error => error.ToString()).ToImmutableList();
    }
}
