using System.Collections.Immutable;
using EMachine.Orleans.Shared.Queries;

namespace EMachine.Sales.Orleans.Queries;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineSearchPagedListQuery(string Criteria, int SkipCount, int MaxResultCount, IImmutableList<KeyValuePair<string, bool>> Sorts, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy)
    : DomainSearchPagedListQuery(Criteria, SkipCount, MaxResultCount, Sorts, TraceId, OperatedAt, OperatedBy);
