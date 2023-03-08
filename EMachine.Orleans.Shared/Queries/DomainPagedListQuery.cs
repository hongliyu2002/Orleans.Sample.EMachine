using System.Collections.Immutable;
using Fluxera.Guards;

namespace EMachine.Orleans.Shared.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainPagedListQuery : DomainListQuery
{
    protected DomainPagedListQuery(int skipCount, int maxResultCount, IImmutableList<KeyValuePair<string, bool>> sortings, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(sortings, traceId, operatedAt, operatedBy)
    {
        SkipCount = Guard.Against.Negative(skipCount, nameof(skipCount));
        MaxResultCount = Guard.Against.NegativeOrZero(maxResultCount, nameof(maxResultCount));
    }

    [Id(0)]
    public int SkipCount { get; }

    [Id(1)]
    public int MaxResultCount { get; }
}
