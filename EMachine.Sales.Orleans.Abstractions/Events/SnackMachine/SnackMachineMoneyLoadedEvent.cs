using EMachine.Sales.Orleans.States;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineMoneyLoadedEvent(Guid Id, Money Money, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version) 
    : SnackMachineEvent(Id, TraceId, OperatedAt, OperatedBy, Version);
