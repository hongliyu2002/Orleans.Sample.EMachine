namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineMoneyUnloadedEvent(Guid Id, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version) 
    : SnackMachineEvent(Id, TraceId, OperatedAt, OperatedBy, Version);
