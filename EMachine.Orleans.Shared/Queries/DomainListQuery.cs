using System.Collections.Immutable;
using Fluxera.Guards;

namespace EMachine.Orleans.Shared.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainListQuery : DomainQuery
{
    protected DomainListQuery(IImmutableList<KeyValuePair<string, bool>> sortings, Guid traceId, DateTimeOffset operatedAt, string operatedBy)
        : base(traceId, operatedAt, operatedBy)
    {
        Sortings = Guard.Against.Null(sortings, nameof(sortings));
    }

    [Id(0)]
    public IImmutableList<KeyValuePair<string, bool>> Sortings { get; }
}
