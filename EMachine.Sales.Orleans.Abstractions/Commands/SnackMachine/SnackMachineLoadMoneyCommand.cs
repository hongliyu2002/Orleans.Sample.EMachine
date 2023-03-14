using EMachine.Orleans.Abstractions.Commands;
using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineLoadMoneyCommand(Money Money, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) 
    : DomainCommand(TraceId, OperatedAt, OperatedBy);
