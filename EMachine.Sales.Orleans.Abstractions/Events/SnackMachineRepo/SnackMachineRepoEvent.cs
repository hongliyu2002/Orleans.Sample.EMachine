using EMachine.Orleans.Shared.Events;

namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public abstract record SnackMachineRepoEvent(Guid Id, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version)
    : DomainEvent(TraceId, OperatedAt, OperatedBy, Version);
