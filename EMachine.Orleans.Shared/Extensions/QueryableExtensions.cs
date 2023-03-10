using System.Collections.Immutable;

namespace EMachine.Orleans.Shared.Extensions;

public static class QueryableExtensions
{
    public static async Task<ImmutableList<TSource>> ToImmutableListAsync<TSource>(this IQueryable<TSource> queryable, CancellationToken cancellationToken = default)
    {
        var result = ImmutableList.CreateBuilder<TSource>();
        await foreach (var element in queryable.ToAsyncEnumerable().WithCancellation(cancellationToken))
        {
            result.Add(element);
        }
        return result.ToImmutable();
    }
}
