using Fluxera.Entity.DomainEvents;

namespace EMachine.Orleans.Shared.Events;

[Immutable]
[GenerateSerializer]
public abstract record DomainEvent(Guid TraceId, DateTimeOffset OperatedAt, string OperatedBy, int Version) 
    : IDomainEvent, ITraceable;
