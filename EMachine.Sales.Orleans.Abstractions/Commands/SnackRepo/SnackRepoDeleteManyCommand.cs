using EMachine.Orleans.Shared.Commands;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRepoDeleteManyCommand(Guid[] Ids, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) 
    : DomainCommand(TraceId, OperatedAt, OperatedBy);
