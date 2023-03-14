namespace EMachine.Orleans.Abstractions.Events;

[Immutable]
[GenerateSerializer]
public abstract record DomainEvent(Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version) : ITraceable;
