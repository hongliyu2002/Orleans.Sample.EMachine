namespace EMachine.Sales.Orleans.Events;

[Immutable]
[GenerateSerializer]
public sealed record SnackMachineRepoCreatedEvent(Guid Id, Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version)
    : SnackMachineRepoEvent(Id, TraceId, OperatedAt, OperatedBy, Version);
