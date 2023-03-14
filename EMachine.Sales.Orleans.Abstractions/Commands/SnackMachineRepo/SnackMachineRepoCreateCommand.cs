using System.Collections.Immutable;
using EMachine.Orleans.Abstractions.Commands;
using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Commands;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRepoCreateCommand(Guid Id, Money MoneyInside, IImmutableList<Slot> Slots, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy) 
    : DomainCommand(TraceId, OperatedAt, OperatedBy);
