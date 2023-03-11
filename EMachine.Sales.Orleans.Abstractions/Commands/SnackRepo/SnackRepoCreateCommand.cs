using EMachine.Orleans.Shared.Commands;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackRepoCreateCommand(Guid Id, string Name, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) 
    : DomainCommand(TraceId, OperatedAt, OperatedBy);
