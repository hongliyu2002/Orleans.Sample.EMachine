using EMachine.Orleans.Abstractions.Events;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public record SnackMachineEvent(Guid Id, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version) 
    : DomainEvent(TraceId, OperatedAt, OperatedBy, Version);
