using System.Collections.Immutable;

namespace EMachine.Orleans.Abstractions.Queries;

[Immutable]
[GenerateSerializer]
public abstract record DomainSearchPagedListQuery(string Criteria, int SkipCount, int MaxResultCount, IImmutableList<KeyValuePair<string, bool>> Sorts, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy)
    : DomainPagedListQuery(SkipCount, MaxResultCount, Sorts, TraceId, OperatedAt, OperatedBy);
