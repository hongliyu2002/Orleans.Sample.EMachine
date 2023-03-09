using System.Collections.Immutable;
using EMachine.Orleans.Shared.Queries;

namespace E.Sales.Orleans.Queries;

[Immutable]
[GenerateSerializer]
public sealed record SnackSearchPagedListQuery(string Criteria, int SkipCount, int MaxResultCount, IImmutableList<KeyValuePair<string, bool>> Sorts, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy)
    : DomainSearchPagedListQuery(Criteria, SkipCount, MaxResultCount, Sorts, TraceId, OperatedAt, OperatedBy);
