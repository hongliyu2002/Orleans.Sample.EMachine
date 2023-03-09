using System.Collections.Immutable;

namespace EMachine.Orleans.Shared.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainPagedListQuery(int SkipCount, int MaxResultCount, IImmutableList<KeyValuePair<string, bool>> Sorts, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy)
    : DomainListQuery(Sorts, TraceId, OperatedAt, OperatedBy);
