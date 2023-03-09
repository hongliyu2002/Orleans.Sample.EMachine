using EMachine.Orleans.Shared.Commands;
using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineLoadSnacksCommand(int Position, SnackPile SnackPile, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) 
    : DomainCommand(TraceId, OperatedAt, OperatedBy);
