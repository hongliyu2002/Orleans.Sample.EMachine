using Orleans.FluentResults;

namespace EMachine.Domain.Shared.Extensions;

/// <summary>
/// </summary>
public static class EnumerableExtensions
{
    public static string ToMessage(this IEnumerable<IError> errors)
    {
        return string.Join(';', errors.OfType<Error>()
                                      .Select(error => error.ToString()));
    }
}
