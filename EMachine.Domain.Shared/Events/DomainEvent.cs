using System;
using Fluxera.Entity.DomainEvents;
using Fluxera.Guards;
using Orleans;

namespace EMachine.Domain.Shared.Events;

[Immutable]
[Serializable]
[GenerateSerializer]
public abstract class DomainEvent : IDomainEvent, ITraceable
{
    protected DomainEvent()
    {
        OperatedBy = string.Empty;
    }

    protected DomainEvent(Guid traceId, string operatedBy)
        : this()
    {
        TraceId = Guard.Against.Empty(traceId, nameof(traceId));
        OperatedBy = Guard.Against.NullOrWhiteSpace(operatedBy, nameof(operatedBy));
    }

    /// <inheritdoc />
    [Id(0)]
    public Guid TraceId { get; set; }

    /// <inheritdoc />
    [Id(1)]
    public string OperatedBy { get; set; }
}
