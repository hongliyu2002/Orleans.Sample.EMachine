using System.Collections.Immutable;
using EMachine.Orleans.Abstractions.Queries;

namespace EMachine.Sales.Orleans.Queries;

[Immutable]
[GenerateSerializer]
public sealed record SnackPagedListQuery(int SkipCount, int MaxResultCount, IImmutableList<KeyValuePair<string, bool>> Sorts, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy)
    : DomainPagedListQuery(SkipCount, MaxResultCount, Sorts, TraceId, OperatedAt, OperatedBy);
